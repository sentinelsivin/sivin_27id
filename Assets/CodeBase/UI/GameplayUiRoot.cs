using CodeBase.Domain.Board;
using CodeBase.Domain.Field;
using UnityEngine;

namespace CodeBase.UI
{
    public class GameplayUiRoot : MonoBehaviour
    {
        [SerializeField] private BoardsPresenter _boards;
        [SerializeField] private FieldPresenter _field;

        public BoardsPresenter Boards => _boards;
        public FieldPresenter Field => _field;
    }
}