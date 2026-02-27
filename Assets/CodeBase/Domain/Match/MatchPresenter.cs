using CodeBase.Data.PlayerDataComponents;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Domain.Match
{
    public sealed class MatchPresenter : MonoBehaviour
    {
        [SerializeField] private GameplayUiRoot _ui;

        private Match _match;

        public void StartMatch(Match match, PlayerId bottom, PlayerId top)
        {
            StopMatch();

            _match = match;

            _ui.Boards.Bind(_match, bottom, top);
            _ui.Field.Bind(_match, bottom, top /* + bottom/top если надо */);
        }

        public void StopMatch()
        {
            // важно: Bind уже подписывает события,
            // а OnDestroy отписывает — но StopMatch нужен для смены режимов/перезапуска без уничтожения объекта.
            // Тогда делай явный Unbind (см. ниже).
            if (_match == null) return;

            _ui.Boards.Unbind();
            _ui.Field.Unbind();

            _match = null;
        }

        private void OnDestroy() => StopMatch();
    }
}