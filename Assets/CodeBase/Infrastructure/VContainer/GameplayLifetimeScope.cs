using CodeBase.Domain;
using CodeBase.Domain.Match;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services;
using CodeBase.Services.FirstTurn;
using CodeBase.Services.GameStart;
using CodeBase.Services.Participants;
using CodeBase.Services.StateMachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BootstrapComponents _bootstrapComponents;
        [SerializeField] private MainSceneMode _mainSceneMode;

        protected override void Configure(IContainerBuilder builder)
        {
            // Scene MonoBehaviours
            builder.RegisterComponentInHierarchy<BootstrapComponents>();
            builder.RegisterComponentInHierarchy<MainSceneMode>();
            builder.RegisterComponentInHierarchy<GameCoordinator>();
            builder.RegisterComponentInHierarchy<GameState>(); // если он на сцене

            // GameStart services
            // builder.Register<ILocalPlayerIdProvider, PersistentLocalPlayerIdProvider>(Lifetime.Singleton);
            // builder.Register<IOpponentIdSource, AiOpponentIdSource>(Lifetime.Singleton);
            builder.Register<IParticipantsProvider, ParticipantsProvider>(Lifetime.Singleton);

            builder.Register<IFirstTurnSelector, LocalFirstTurnSelector>(Lifetime.Singleton);

            builder.Register<StartGameCoordinator>(Lifetime.Singleton);

            // Domain
            builder.Register<IMatchRules, DefaultMatchRules>(Lifetime.Singleton);
            builder.Register<MatchFactory>(Lifetime.Singleton);

            // Единственный entry point игровой сцены
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }

}