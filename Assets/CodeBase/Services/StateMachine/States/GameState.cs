using CodeBase.Services.GameStart;
using VContainer;

namespace CodeBase.Services.StateMachine.States
{
    public class GameState : StateMachineBehavior
    {
   
        [Inject] private StartGameCoordinator _startGame;
        [Inject] private GameCoordinator _gameCoordinator;

        protected override void OnEnter()
        {
            var config = _startGame.CreateConfig(GameMode.VsAi);
            _gameCoordinator.StartGame(config);
        }

        protected override void OnExit()
        {
            _gameCoordinator.StopGame();
        }
        
    }
}
