using CodeBase.Services.StateMachine.States;

namespace CodeBase.Services.StateMachine
{
    public class MainSceneMode : StateModeBehavior
    {
        public void GoToStartGame() => ChangeState<StartGameState>();
        public void GoToGame() => ChangeState<GameState>();
    }
}
