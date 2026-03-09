using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Services.Turn;

namespace CodeBase.Temporarily
{
    public class NoopPlayerController : IPlayerController
    {
        public async Task<CellPosition> RequestPlacementAsync(PlayerId playerId, CancellationToken ct)
        {
            // ждём отмены токена (StopGame)
            await Task.Delay(Timeout.Infinite, ct);
            return default;
        }
        
        // когда будет реализован UiPlayerController и  AiController данный класс перестанет быть нужен 
        // т.к сейчас используется для проверки другого функционала
    }
}