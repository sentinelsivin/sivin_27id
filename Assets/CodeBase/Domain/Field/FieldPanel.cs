using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _localPlayerRoot;
        [SerializeField] private RectTransform _opponentPlayeRoot;
        
        [SerializeField] private FieldViewFactory _fieldFactory;

        private FieldView _localPlayerView;
        private FieldView _opponentPlayerView;

        public void EnsureCreated()
        {
            if (_localPlayerView == null) _localPlayerView = _fieldFactory.Create(_localPlayerRoot);
            if (_opponentPlayerView == null) _opponentPlayerView = _fieldFactory.Create(_opponentPlayeRoot);
        }

        public FieldView Get(PlayerSlot slot)
        {
            EnsureCreated();
            return slot == PlayerSlot.Local ? _localPlayerView : _opponentPlayerView;
        }

        public void ClearAll()
        {
            if (_localPlayerView != null) _localPlayerView.Clear();
            if (_opponentPlayerView != null) _opponentPlayerView.Clear();
        }
    }
    
}