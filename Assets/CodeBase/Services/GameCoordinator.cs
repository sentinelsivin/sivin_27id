using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Services.GameStart;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameCoordinator : MonoBehaviour
    {
        [SerializeField] private MatchPresenter _matchPresenter;

        private MatchFactory _matchFactory;
        private IMatchRules _rules;
        private Match _match;
        
        private PlayerId _playerFirst;
        private PlayerId _playerSecond;

        public void Initialize(PlayerId playerFirst, PlayerId playerSecond)
        {
            _playerFirst = playerFirst;
            _playerSecond = playerSecond;
            
            _matchFactory = new MatchFactory(_rules);
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