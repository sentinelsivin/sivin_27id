using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Services.Turn
{
    public class AiController : IPlayerController
    {
        public Task<CellPosition> RequestPlacementAsync(PlayerId playerId, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }
    }
}