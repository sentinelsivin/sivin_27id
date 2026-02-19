using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Flow
{
    public sealed class TurnSystem
    {
        private readonly Match _match;
        private readonly IReadOnlyDictionary<PlayerId, IPlayerController> _controllers;

        public TurnSystem(Match match, IReadOnlyDictionary<PlayerId, IPlayerController> controllers)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
            _controllers = controllers ?? throw new ArgumentNullException(nameof(controllers));
        }

        public async Task PlayTurnAsync(CancellationToken ct)
        {
            var player = _match.TurnOrder.ActivePlayer;

            // 1) бросок кости
            _match.RollDice(player);

            // 2) запросить у контроллера позицию (UI/AI)
            var ctrl = _controllers[player];
            var pos = await ctrl.RequestPlacementAsync(player, ct);

            // 3) попытаться поставить
            if (!_match.TryPlaceDice(player, pos))
            {
                // минимально: если ход невалиден — повтор
                // (потом можно сделать лимит попыток/сообщения)
                return;
            }

            // 4) завершить ход
            if (!_match.IsFinished)
                _match.EndTurn();
        }
    }
    
}