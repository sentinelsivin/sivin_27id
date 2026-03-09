using CodeBase.Data.PlayerDataComponents;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Domain.Match
{
    public class MatchPresenter : MonoBehaviour
    {
        [SerializeField] private GameplayUiRoot _ui;

        private IMatchReadModel _model;

        public void StartMatch(IMatchReadModel model, PlayerId localPlayer, PlayerId opponentPlayer)
        {
            StopMatch();

            _model = model;

            _ui.Boards.Bind(_model, localPlayer, opponentPlayer);
            //_ui.Field.Bind(_model, localPlayer, opponentPlayer);
        }

        public void StopMatch()
        {
            if (_model == null) return;

            _ui.Boards.Unbind();
           // _ui.Field.Unbind();

            _model = null;
        }

        private void OnDestroy() => StopMatch();
    }
}