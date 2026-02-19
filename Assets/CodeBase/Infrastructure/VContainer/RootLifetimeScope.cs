using CodeBase.Infrastructure.DataProvider;
using CodeBase.Infrastructure.SceneLoad;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.VContainer
{
    public  class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingScreen _loadingScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            // Данные/инфраструктура (живут всегда)
            
            builder.RegisterInstance(new SceneCatalog());
            
            builder.RegisterComponent(_coroutineRunner).As<ICoroutineRunner>();
            builder.RegisterComponent(_loadingScreen).As<ILoadingScreen>();
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            
            builder.RegisterComponent(_bootstrapper);
            
            builder.Register<IDataBootstrap, DataBootstrap>(Lifetime.Singleton);
            builder.Register<IStartupSequence, StartupSequence>(Lifetime.Singleton);

            builder.Register<IPersistentData, PersistentData>(Lifetime.Singleton);
            builder.Register<IDataProvider, DataLocalProvider>(Lifetime.Singleton);


            // Сервис загрузки сцен

        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            _bootstrapper.Run();
        }

    }
}