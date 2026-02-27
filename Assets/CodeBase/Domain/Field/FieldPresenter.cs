using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPresenter : MonoBehaviour
    {
        [SerializeField] private FieldPanel _fieldPanel;

        private Match.Match _match;
        private PlayerId _bottom;
        private PlayerId _top;

        public void Bind(Match.Match match, PlayerId bottom, PlayerId top)
        {
            Unbind();

            _match = match;
            _bottom = bottom;
            _top = top;

            _fieldPanel.EnsureCreated();
            _fieldPanel.ClearAll();

            _match.DicePlaced += OnDicePlaced;
        }

        public void Unbind()
        {
            if (_match != null)
                _match.DicePlaced -= OnDicePlaced;

            _match = null;
        }

        private void OnDicePlaced(PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos)
        {
            if (_match == null) return;

            var slot = playerId.Equals(_bottom) ? PlayerSlot.Bottom : PlayerSlot.Top;
            var view = _fieldPanel.Get(slot);

            view.PlaceDice(dice, pos); // метод делаешь в FieldView
        }

        private void OnDestroy() => Unbind();
    }
}