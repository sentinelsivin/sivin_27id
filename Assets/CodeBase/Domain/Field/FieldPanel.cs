using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPanel : MonoBehaviour
    {
        [SerializeField] private FieldViewFactory _fieldFactory;
        [SerializeField] private RectTransform _filedFirstPanel;
        [SerializeField] private RectTransform _filedSecondPanel;

        private FieldView _bottom;
        private FieldView _top;

        public void EnsureCreated()
        {
            if (_bottom == null) _bottom = _fieldFactory.Create(_filedFirstPanel);
            if (_top == null) _top = _fieldFactory.Create(_filedSecondPanel);
        }

        public FieldView Get(PlayerSlot slot)
        {
            EnsureCreated();
            return slot == PlayerSlot.Bottom ? _bottom : _top;
        }

        public void ClearAll()
        {
            if (_bottom != null) _bottom.Clear();
            if (_top != null) _top.Clear();
        }
    }

    public enum PlayerSlot { Bottom, Top }
}