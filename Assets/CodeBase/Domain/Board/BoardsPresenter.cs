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

        private IMatchReadModel _model;

        public void Bind(IMatchReadModel model, PlayerId localPlayer, PlayerId opponentPlayer)
        {
            Unbind();

            _model = model;

            _slotByPlayer[localPlayer] = BoardSlot.Local;
            _slotByPlayer[opponentPlayer] = BoardSlot.Opponent;
            
            _viewBySlot[BoardSlot.Local] = _playerBoardView;
            _viewBySlot[BoardSlot.Opponent] = _opponentBoardView;

            _model.DiceChanged += OnDiceChanged;

            // initial sync
            OnDiceChanged(localPlayer, _model.GetDice(localPlayer));
            OnDiceChanged(opponentPlayer, _model.GetDice(opponentPlayer));
        }
        
        public void Unbind()
        {
            if (_model != null)
                _model.DiceChanged -= OnDiceChanged;

            _model = null;
            _slotByPlayer.Clear();
            _viewBySlot.Clear();
        }

        private void OnDiceChanged(PlayerId playerId, Dice.Dice dice)
        {
            if (!_slotByPlayer.TryGetValue(playerId, out var slot))
            {
                return;
            }

            if (!_viewBySlot.TryGetValue(slot, out var view))
            {
                return;
            }
            
            view.EnsureDiceView(dice);
        }

        private void OnDestroy()
        {
            if (_model != null)
                _model.DiceChanged -= OnDiceChanged;
        }
    }
}