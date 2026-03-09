using CodeBase.Domain;
using CodeBase.Domain.Match;
using CodeBase.Domain.Match.Rules;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services;
using CodeBase.Services.Controllers;
using CodeBase.Services.FirstTurn;
using CodeBase.Services.GameStart;
using CodeBase.Services.Participants;
using CodeBase.Services.StateMachine;
using CodeBase.Services.StateMachine.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private BootstrapComponents _bootstrapComponents;
        [SerializeField] private MainSceneMode _mainSceneMode;
        [SerializeField] private GameCoordinator _gameCoordinator;
        [SerializeField] private GameplayState gameplayState;

        protected override void Configure(IContainerBuilder builder)
        {
            // Scene MonoBehaviours
            builder.RegisterComponentInHierarchy<BootstrapComponents>();
            builder.RegisterComponentInHierarchy<MainSceneMode>();
            builder.RegisterComponentInHierarchy<GameCoordinator>();
            builder.RegisterComponentInHierarchy<GameplayState>(); // если он на сцене

            // GameStart services
            builder.Register<ILocalPlayerIdProvider, PersistentLocalPlayerIdProvider>(Lifetime.Singleton);
            builder.Register<IOpponentIdSource, AiOpponentIdSource>(Lifetime.Singleton);
            builder.Register<IParticipantsProvider, ParticipantsProvider>(Lifetime.Singleton);

            builder.Register<IFirstTurnSelector, LocalFirstTurnSelector>(Lifetime.Singleton);

            builder.Register<GameStartConfigFactory>(Lifetime.Singleton);

            // Domain
            builder.Register<IMatchRules, DefaultMatchRules>(Lifetime.Singleton);
            builder.Register<MatchFactory>(Lifetime.Singleton);
            
            builder.Register<IControllerFactory, VsAiControllerFactory>(Lifetime.Singleton);

            // Единственный entry point игровой сцены
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }

}