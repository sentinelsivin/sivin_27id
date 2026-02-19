using System;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Domain.Match;

namespace CodeBase.Services.Flow
{
    public sealed class GameFlow
    {
        private readonly Match _match;
        private readonly TurnSystem _turnSystem;

        public GameFlow(Match match, TurnSystem turnSystem)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
            _turnSystem = turnSystem ?? throw new ArgumentNullException(nameof(turnSystem));
        }

        public async Task RunAsync(CancellationToken ct)
        {
            while (!_match.IsFinished && !ct.IsCancellationRequested)
            {
                await _turnSystem.PlayTurnAsync(ct);

                // сюда позже добавить:
                // await _presentationGate.WaitForTurnPresentationAsync(ct);
            }
        }
    }
    
}