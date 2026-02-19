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
            builder.RegisterComponentInHierarchy<BootstrapComponents>();
            builder.RegisterComponentInHierarchy<MainSceneMode>();

            builder.RegisterEntryPoint<GameplayEntryPoint>();

            // сюда же: фабрики, presenters, scene services и т.п.
        }
    }
}