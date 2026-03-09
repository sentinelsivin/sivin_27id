using CodeBase.Domain.Match;
using CodeBase.Services.Controllers;
using CodeBase.Services.Flow;
using CodeBase.Services.GameStart;
using UnityEngine;
using VContainer;

namespace CodeBase.Services
{
    public class GameCoordinator : MonoBehaviour
    {
        [SerializeField] private MatchPresenter _matchPresenter;

        private MatchFactory _matchFactory;
        private Match _match;
        
        private IControllerFactory _controllerFactory;
        
        private GameFlow _gameFlow;

        [Inject]
        public void Construct(MatchFactory matchFactory, IControllerFactory controllerFactory)
        {
            _matchFactory = matchFactory;
            _controllerFactory = controllerFactory;
        }

        public void StartGame(GameStartConfig config)
        {

            StopGame();
            
            var players = new[] { config.Participants.Local, config.Participants.Opponent };

            _match = _matchFactory.Create(players,  config.FirstTurnPlayer);
            _matchPresenter.StartMatch(_match, config.Participants.Local, config.Participants.Opponent); // наверное, оптимальней было бы сделать обертку
            
            var controllers = _controllerFactory.CreateControllers(config);
            
            var turnSystem = new TurnSystem(_match, controllers, 1);
            _gameFlow = new GameFlow(_match, turnSystem);
            _gameFlow.Start();
        }

        public void StopGame()
        {
            _gameFlow?.Stop();
            _gameFlow = null;

            _matchPresenter.StopMatch();
            _match = null;
        }
    }
}