using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
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

        [Inject]
        public void Construct(MatchFactory matchFactory)
        {
            _matchFactory = matchFactory;
        }
        public void StartGame(GameStartConfig config)
        {
            var players = new[] { config.Participants.Local, config.Participants.Opponent };

            _match = _matchFactory.Create(players);
            _matchPresenter.StartMatch(_match, config.Participants.Local, config.Participants.Opponent);
        }

        public void StopGame()
        {
            _matchPresenter.StopMatch();
            _match = null;
        }
    }
}