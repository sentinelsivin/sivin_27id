using CodeBase.Services.StateMachine;
using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace CodeBase.Infrastructure
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