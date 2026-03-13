using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPresenter : MonoBehaviour
    {
        [SerializeField] private FieldPanel _fieldPanel;

        private IMatchReadModel _match;
        private PlayerSlotResolver _slotResolver;

        public void Bind(IMatchReadModel match, PlayerSlotResolver slotResolver)
        {
            Unbind();

            _match = match;
            _slotResolver = slotResolver;

            if (_match.Players == null || _match.Players.Count == 0)
                throw new InvalidOperationException("Match does not contain players.");

            PlayerId anyPlayerId = _match.Players[0];
            Field field = _match.GetField(anyPlayerId);

            _fieldPanel.EnsureCreated(field.ColumnsCount, field.RowsCount);
            _fieldPanel.ClearAll();

            _match.DicePlaced += OnDicePlaced;
        }

        public void Unbind()
        {
            if (_match != null)
                _match.DicePlaced -= OnDicePlaced;

            _match = null;
            _slotResolver = null;
        }

        private void OnDicePlaced(PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos)
        {
            if (_match == null || _slotResolver == null)
                return;

            var slot = _slotResolver.Resolve(playerId);
            var view = _fieldPanel.Get(slot);

            // Пока ничего не делаем:
            // view.PlaceDiceView(..., pos);
        }

        private void OnDestroy() => Unbind();
    }
}