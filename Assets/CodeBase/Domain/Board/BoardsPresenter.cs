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

        private readonly Dictionary<PlayerSlot, BoardView> _viewBySlot = new();

        private IMatchReadModel _model;
        private PlayerSlotResolver _slotResolver;

        public void Bind(IMatchReadModel model, PlayerSlotResolver slotResolver)
        {
            Unbind();

            _model = model;
            _slotResolver = slotResolver;

            _viewBySlot[PlayerSlot.Local] = _playerBoardView;
            _viewBySlot[PlayerSlot.Opponent] = _opponentBoardView;

            _model.DiceChanged += OnDiceChanged;

            OnDiceChanged(_slotResolver.LocalPlayer, _model.GetDice(_slotResolver.LocalPlayer));
            OnDiceChanged(_slotResolver.OpponentPlayer, _model.GetDice(_slotResolver.OpponentPlayer));
        }

        public void Unbind()
        {
            if (_model != null)
                _model.DiceChanged -= OnDiceChanged;

            _model = null;
            _slotResolver = null;
            _viewBySlot.Clear();
        }

        private void OnDiceChanged(PlayerId playerId, Dice.Dice dice)
        {
            if (_slotResolver == null)
                return;

            var slot = _slotResolver.Resolve(playerId);

            if (_viewBySlot.TryGetValue(slot, out var view))
                view.EnsureDiceView(dice);
        }

        private void OnDestroy() => Unbind();
    }
}