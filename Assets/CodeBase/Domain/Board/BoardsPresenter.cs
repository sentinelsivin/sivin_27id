using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardsPresenter : MonoBehaviour
    {
        [SerializeField] private BoardView _playerBoardView;
        [SerializeField] private BoardView _opponentBoardView;

        private readonly Dictionary<PlayerId, BoardSlot> _slotByPlayer = new();
        private readonly Dictionary<BoardSlot, BoardView> _viewBySlot = new();

        private IMatchReadModel _match;

        public void Bind(IMatchReadModel match, PlayerId localPlayer, PlayerId opponentPlayer)
        {
            _match = match;

            _viewBySlot[BoardSlot.Player] = _playerBoardView;
            _viewBySlot[BoardSlot.Opponent] = _opponentBoardView;

            _slotByPlayer[localPlayer] = BoardSlot.Player;
            _slotByPlayer[opponentPlayer] = BoardSlot.Opponent;

            _match.DiceChanged += OnDiceChanged;

            // очистить UI на старте
            _playerBoardView.EnsureDiceView(null);
            _opponentBoardView.EnsureDiceView(null);
        }
        
        public void Unbind()
        {
            if (_match != null)
                _match.DiceChanged -= OnDiceChanged;

            _match = null;
            _slotByPlayer.Clear();
            _viewBySlot.Clear();
        }

        private void OnDiceChanged(PlayerId playerId, Dice.Dice dice)
        {
            if (!_slotByPlayer.TryGetValue(playerId, out var slot))
                return;

            _viewBySlot[slot].EnsureDiceView(dice);
        }

        private void OnDestroy()
        {
            if (_match != null)
                _match.DiceChanged -= OnDiceChanged;
        }
    }
}