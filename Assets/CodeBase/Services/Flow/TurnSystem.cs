using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Flow
{
    public class TurnSystem
    {
        private readonly Match _match;
        private readonly IReadOnlyDictionary<PlayerId, IPlayerController> _controllers;
        private readonly int _maxAttempts;


        public TurnSystem(Match match, IReadOnlyDictionary<PlayerId, IPlayerController> controllers, int maxAttempts)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
            _controllers = controllers ?? throw new ArgumentNullException(nameof(controllers));
            _maxAttempts = Math.Max(1, maxAttempts);
        }

        public async Task PlayTurnAsync(CancellationToken ct)
        {
            var player = _match.TurnOrder.ActivePlayer;

            // 1) бросок кости
            _match.RollDice(player);

            // 2) запросить у контроллера позицию (UI/AI)
            var ctrl = _controllers[player];
            for (int attempt = 0; attempt < _maxAttempts && !_match.IsFinished; attempt++)
            {
                ct.ThrowIfCancellationRequested();

                var pos = await ctrl.RequestPlacementAsync(player, ct);

                if (_match.TryPlaceDice(player, pos))
                {
                    if (!_match.IsFinished)
                        _match.EndTurn();
                    return;
                }

                // optional: дать контроллеру знать, что попытка невалидна
                // ctrl.NotifyInvalidMove(player, pos);
            }

            // 4) завершить ход
            if (!_match.IsFinished)
                _match.EndTurn();
        }
    }
    
}