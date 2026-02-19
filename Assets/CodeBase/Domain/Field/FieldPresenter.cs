using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPresenter : MonoBehaviour
    {
        [SerializeField] private FieldPanel _fieldPanel;

        private Match.Match _match;

        public void Bind(Match.Match match)
        {
            _match = match;
            _match.DicePlaced += OnDicePlaced;
        }

        private void OnDicePlaced(CodeBase.Data.PlayerDataComponents.PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos)
        {
            // TODO: обновить FieldView нужного игрока через _fieldPanel / фабрики
            // здесь будет визуализация установки dice на клетку
        }

        private void OnDestroy()
        {
            if (_match != null)
                _match.DicePlaced -= OnDicePlaced;
        }
    }
}