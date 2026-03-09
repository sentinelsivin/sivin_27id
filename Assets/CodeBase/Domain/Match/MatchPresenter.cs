using CodeBase.Data.PlayerDataComponents;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Domain.Match
{
    public class MatchPresenter : MonoBehaviour
    {
        [SerializeField] private GameplayUiRoot _uiRoot;

        private IMatchReadModel _model;
        private PlayerSlotResolver _slotResolver;

        public void StartMatch(IMatchReadModel model, PlayerId localPlayer, PlayerId opponentPlayer)
        {
            StopMatch();

            _model = model;
            _slotResolver = new PlayerSlotResolver(localPlayer, opponentPlayer);

            _uiRoot.Boards.Bind(_model, _slotResolver);
            _uiRoot.Field.Bind(_model, _slotResolver);
        }

        public void StopMatch()
        {
            if (_model == null) return;

            _uiRoot.Boards.Unbind();
            _uiRoot.Field.Unbind();

            _model = null;
        }

        private void OnDestroy() => StopMatch();
    }
}