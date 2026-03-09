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

            _fieldPanel.EnsureCreated();
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

            view.PlaceDice(dice, pos);
        }

        private void OnDestroy() => Unbind();
    }
}