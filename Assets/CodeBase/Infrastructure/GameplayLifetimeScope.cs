using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BootstrapComponents _bootstrapComponents;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<BootstrapComponents>();

            // сюда же: фабрики, presenters, scene services и т.п.
        }
    }
}