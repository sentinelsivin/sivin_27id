using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPanel : MonoBehaviour
    {
   
        [SerializeField] private FieldViewFactory _fieldFactory;
        [SerializeField] private RectTransform _filedFirstPanel;
        [SerializeField] private RectTransform _filedSecondPanel;
        
        private FieldView _fieldFirstView;
        private FieldView _fieldSecondView;
    }
}
