using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _localPlayerRoot;
        [SerializeField] private RectTransform _opponentPlayerRoot;
        [SerializeField] private FieldViewFactory _fieldFactory;

        private FieldView _localPlayerView;
        private FieldView _opponentPlayerView;

        public void EnsureCreated(int columnsCount, int rowsCount)
        {
            if (_localPlayerView == null)
                _localPlayerView = _fieldFactory.Create(_localPlayerRoot, columnsCount, rowsCount);

            if (_opponentPlayerView == null)
                _opponentPlayerView = _fieldFactory.Create(_opponentPlayerRoot, columnsCount, rowsCount);
        }

        public FieldView Get(PlayerSlot slot)
        {
            return slot == PlayerSlot.Local ? _localPlayerView : _opponentPlayerView;
        }

        public void ClearAll()
        {
            if (_localPlayerView != null)
                _localPlayerView.Clear();

            if (_opponentPlayerView != null)
                _opponentPlayerView.Clear();
        }
    }
}