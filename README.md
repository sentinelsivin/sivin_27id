# Dice Battle (Unity) — Документация по проекту

Этот документ описывает **текущую архитектуру**, **ответственности модулей** и **поток зависимостей во время запуска** для проекта Dice Battle.  
Основной реализуемый режим: **Vs AI** (остальные режимы подготовлены на будущее: локальный 2 игрока, онлайн).

---

## 1) Цели и принципы архитектуры

**Цели**
- Держать **Domain** “чистым”: без зависимостей от Unity (`MonoBehaviour`, `GameObject`, `Time` и т.п.).
- Управлять боем через **слой оркестрации** (Services), который композит Domain и Unity.
- Делать режимы (VsAi / Local / Online) **переключаемыми** через небольшие стратегии и DI.

**Принципы**
- SRP / Clean Code / границы модулей.
- Направление зависимостей: `Unity/Infrastructure → Services → Domain`.
- Сборка зависимостей через VContainer.

---

## 2) Карта модулей на верхнем уровне

### Слои

| Слой | Что содержит | Чего НЕ должно быть |
|---|---|---|
| **Domain** (`CodeBase.Domain`) | сущности, правила, логика матча | Unity, сохранения, сеть |
| **Services** (`CodeBase.Services`) | сборка конфигов, выбор участников, фабрики контроллеров, выбор первого хода, state machine states | Unity-вьюхи/объекты сцены, прямой UI |
| **Infrastructure** (`CodeBase.Infrastructure`) | DI scopes, провайдеры данных, bootstrap | доменные правила |
| **Unity слой сцены** (MonoBehaviours) | `BootstrapComponents`, `MainSceneMode`, `GameCoordinator`, объекты state machine на сцене | “чистая” доменная логика внутри компонентов (кроме мостов/адаптеров) |

---

## 3) Текущий запуск игры (Runtime Flow)

**Ключевая идея:** матч запускается из **GameState**, который собирает конфиг и отдаёт его координатору.

### Поток

1. `BootstrapComponents.Run()`  
   вызывает:
   - `_mainSceneMode.Bootstrap()`
   - `_mainSceneMode.GoToGame()`

2. State machine входит в **GameState** (`OnEnter`).
3. `GameState` создаёт `GameStartConfig`:
   - `_gameStartConfigFactory.CreateConfig(GameMode.VsAi)`
4. `GameState` стартует игру:
   - `_gameCoordinator.StartGame(config)`
5. `GameCoordinator` поднимает pipeline матча (создание матча через фабрику, правила, контроллеры под режим — зависит от реализации координатора).

### Почему это хорошо
- **GameCoordinator** больше не решает “кто с кем играет” и “какой режим”.
- Логика старта матча централизована и тестируема через фабрику и провайдеры.

---

## 4) Ключевые понятия

### 4.1 GameStartConfig

`GameStartConfig` — **единый объект**, который описывает всё нужное для запуска матча.

Обычно включает:
- `GameMode`
- `MatchParticipants` (локальный игрок + оппонент)
- политику выбора первого хода
- фабрику контроллеров под режим (например, VsAi)

**Плюсы**
- `GameCoordinator.StartGame(config)` получает один объект, а не множество параметров.
- Легко расширять без ломки сигнатур.

---

## 5) Участники матча: кто локальный игрок, кто оппонент

### 5.1 Интерфейсы

#### `ILocalPlayerIdProvider`
Возвращает **id локального игрока** из персистентных данных.

Текущая реализация:
- `PersistentLocalPlayerIdProvider : ILocalPlayerIdProvider`
  - использует `IPersistentData` → `PlayerData.Id`

#### `IOpponentIdSource`
Возвращает **id оппонента** (зависит от режима).

Текущие/будущие реализации:
- `AiOpponentIdSource` (VsAi)
- `SecondLocalOpponentIdSource` (локальный 2P)
- `OnlineOpponentIdSource` (онлайн) — зависит от lobby/network сервиса

#### `IParticipantsProvider`
Собирает `MatchParticipants`.

Реализация:
- `ParticipantsProvider(ILocalPlayerIdProvider, IOpponentIdSource)`
  - валидирует, что localId != opponentId
  - возвращает `new MatchParticipants(localId, opponentId)`

### 5.2 Почему нужны оба провайдера даже для VsAi
Даже в VsAi всё равно нужно:
- id локального игрока (из сохранённых данных)
- id оппонента (AI-идентификатор/профиль)

То есть эти зависимости — часть **текущего** пайплайна, не только “на будущее”.

---

## 6) Выбор первого хода

### Интерфейс
- `IFirstTurnSelector`
  - выбирает, кто ходит первым

Реализации:
- `LocalFirstTurnSelector`
- `RandomFirstTurnSelector`

Сейчас в DI выбирается один selector глобально (позже можно выбирать по режиму/конфигу).

---

## 7) Контроллеры под режимы (Factories)

Чтобы разные режимы имели разные input/поведение, используется фабрика контроллеров.

### Интерфейс
- `IControllerFactory`
  - строит контроллеры, необходимые для запуска матча под конкретный режим/конфиг

Примеры фабрик:
- `VsAiControllerFactory`
- `OnlineControllerFactory`

Это разгружает `GameCoordinator`: он просит фабрику собрать нужные контроллеры.

---

## 8) Domain: правила и создание матча

### Текущие доменные регистрации
- `IMatchRules` → `DefaultMatchRules`
- `MatchFactory`

`MatchFactory` создаёт доменную модель матча, используя правила и участников.  
(Конкретная структура матча зависит от текущей доменной реализации; важно, что домен остаётся чистым.)

---

## 9) Граф зависимостей (важно)

### Разрешено
```
Infrastructure/Unity → Services → Domain
Infrastructure/Unity → Services
Services → Domain
```

### Запрещено
```
Domain → Unity
Domain → Infrastructure
```

### Практическое правило
Если класс в Domain — он должен компилироваться в обычном .NET проекте без Unity.

---

## 10) VContainer: композиция

### GameplayLifetimeScope (игровая сцена)

Регистрируется:
- Scene MonoBehaviours:
  - `BootstrapComponents`
  - `MainSceneMode`
  - `GameCoordinator`
  - `GameState` (если он лежит на сцене)
- GameStart сервисы:
  - `ILocalPlayerIdProvider` → `PersistentLocalPlayerIdProvider`
  - `IOpponentIdSource` → `AiOpponentIdSource` (по умолчанию)
  - `IParticipantsProvider` → `ParticipantsProvider`
  - `IFirstTurnSelector` → `LocalFirstTurnSelector` (или Random)
  - `GameStartConfigFactory`
- Domain:
  - `IMatchRules` → `DefaultMatchRules`
  - `MatchFactory`
- Entry point:
  - `GameplayEntryPoint` (единая точка входа игровой сцены)

### Что делает `RegisterComponentInHierarchy<T>()`
Это **регистрирует уже существующий MonoBehaviour со сцены** в DI, чтобы:
- его можно было инжектить в другие сервисы, и/или
- ему самому можно было заинжектить зависимости через `[Inject]`.

Это **не** “запускает” компонент (не вызывает `OnEnter()` и т.п.).

---

## 11) Рекомендуемая структура папок (ориентир)

```
CodeBase/
  Domain/
    Match/
    Rules/
    Field/ ...
  Services/
    GameStart/            // сборка конфигов, GameStartConfig, GameMode
    Participants/         // IParticipantsProvider + источники оппонента
    FirstTurn/            // IFirstTurnSelector реализации
    Controllers/          // фабрики контроллеров под режимы
    StateMachine/
      States/             // GameState и т.п.
  Infrastructure/
    VContainer/           // LifetimeScopes
    DataProvider/         // провайдеры id/персистентных данных
    SceneLoad/ ...
  Unity/
    (объекты сцены / view / presenters)
```

### Небольшое улучшение (необязательное)
`IOnlineLobby` лучше держать в отдельном файле/модуле (например, `Services/Online/`), а не внутри `OnlineOpponentIdSource.cs`.

---

## 12) Расширение режимов (на будущее)

### 12.1 Локальный 2 игрока
1. Включить `SecondLocalOpponentIdSource`
2. Поменять DI биндинг `IOpponentIdSource` для режима Local:
   - отдельным LifetimeScope, или
   - позже — резолвером по `GameMode`

### 12.2 Онлайн
1. Реализовать `IOnlineLobby`, инжектить в `OnlineOpponentIdSource`
2. Биндить `IOpponentIdSource` → `OnlineOpponentIdSource`
3. Использовать `OnlineControllerFactory`

---

## 13) Чек-лист частых проблем

- `ParticipantsProvider` требует **оба**: `ILocalPlayerIdProvider` и `IOpponentIdSource`.  
  Если DI биндингов нет → ошибка контейнера при запуске.
- Убедись, что `AiOpponentIdSource` возвращает id, отличный от local id.
- Если `GameState` использует `[Inject]` поля — он должен быть зарегистрирован/инжектнут через DI-механизм.

---

## 14) Быстрый справочник: ответственность ключевых классов

| Компонент | Ответственность |
|---|---|
| `BootstrapComponents` | бутстрап сцены, перевод в игровой state |
| `MainSceneMode` | переходы/роутинг в игровую сцену/state |
| `GameState` | OnEnter собирает конфиг и стартует игру, OnExit — стоп |
| `GameStartConfigFactory` | сборка конфига по `GameMode` и провайдерам |
| `ParticipantsProvider` | сборка `MatchParticipants` (local + opponent) |
| `PersistentLocalPlayerIdProvider` | читает id игрока из persistent data |
| `AiOpponentIdSource` | отдаёт opponent id для AI режима |
| `IFirstTurnSelector` | выбирает первого игрока |
| `IControllerFactory` | собирает контроллеры под режим |
| `MatchFactory` | создаёт доменный матч |
| `IMatchRules` | доменные правила/валидации |

---

## 15) Текущий режим по умолчанию

По решению для фокуса обучения:  
**Default GameMode = Vs AI** (остальные — заготовки).

---

## 16) Следующие шаги (рекомендуемые)

1. Проверить, что `GameCoordinator.StartGame(config)` — это только оркестрация:
   - потребляет config
   - просит фабрики собрать контроллеры
   - создаёт матч через `MatchFactory`
2. Запустить turn loop через:
   - решения контроллеров (AI vs local)
   - проверки через `IMatchRules`
3. Обновление View держать в Unity слое, триггерить из сервисов событиями/вызовами.

---

### Приложение: минимальные DI биндинги для Vs AI

```csharp
builder.Register<ILocalPlayerIdProvider, PersistentLocalPlayerIdProvider>(Lifetime.Singleton);
builder.Register<IOpponentIdSource, AiOpponentIdSource>(Lifetime.Singleton);
builder.Register<IParticipantsProvider, ParticipantsProvider>(Lifetime.Singleton);

builder.Register<IFirstTurnSelector, LocalFirstTurnSelector>(Lifetime.Singleton);
builder.Register<GameStartConfigFactory>(Lifetime.Singleton);

builder.Register<IMatchRules, DefaultMatchRules>(Lifetime.Singleton);
builder.Register<MatchFactory>(Lifetime.Singleton);
```
