using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Controllers
{
    
    public interface IControllerFactory
    {
        IReadOnlyDictionary<PlayerId, IPlayerController> CreateControllers(GameStartConfig config);
    }
}