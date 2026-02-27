using CodeBase.Domain.Board;
using CodeBase.Domain.Field;
using UnityEngine;

namespace CodeBase.UI
{
    public class GameplayUiRoot : MonoBehaviour
    {
        public BoardsPresenter Boards { get; private set; }
        public FieldPresenter Field { get; private set; }
    }
}