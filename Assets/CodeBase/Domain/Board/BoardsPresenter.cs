using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardsPresenter : MonoBehaviour
    {
        [SerializeField] private BoardView _bottomBoardView;
        [SerializeField] private BoardView _topBoardView;

        private readonly Dictionary<PlayerId, BoardSlot> _slotByPlayer = new();
        private readonly Dictionary<BoardSlot, BoardView> _viewBySlot = new();

        private Match.Match _match;

        public void Bind(Match.Match match, PlayerId bottomPlayer, PlayerId topPlayer)
        {
            _match = match;

            _viewBySlot[BoardSlot.Bottom] = _bottomBoardView;
            _viewBySlot[BoardSlot.Top] = _topBoardView;

            _slotByPlayer[bottomPlayer] = BoardSlot.Bottom;
            _slotByPlayer[topPlayer] = BoardSlot.Top;

            _match.DiceChanged += OnDiceChanged;

            // очистить UI на старте
            _bottomBoardView.EnsureDiceView(null);
            _topBoardView.EnsureDiceView(null);
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