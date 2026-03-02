using CodeBase.Services.GameStart;
using VContainer;

namespace CodeBase.Services.StateMachine.States
{
    public class GameplayState : StateMachineBehavior
    {
   
        [Inject] private GameStartConfigFactory _gameStartGame;
        [Inject] private GameCoordinator _gameCoordinator;

        protected override void OnEnter()
        {
            var config = _gameStartGame.CreateConfig(GameMode.VsAi);
            _gameCoordinator.StartGame(config);
        }

        protected override void OnExit()
        {
            _gameCoordinator.StopGame();
        }
        
    }
}
