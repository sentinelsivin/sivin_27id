using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Controllers
{
    public class OnlineControllerFactory : IControllerFactory
    {
        private readonly IPlayerController _localController;
        private readonly IPlayerController _networkController;

        public OnlineControllerFactory(IPlayerController localController, IPlayerController networkController)
        {
            _localController = localController;
            _networkController = networkController;
        }

        public IReadOnlyDictionary<PlayerId, IPlayerController> CreateControllers(GameStartConfig config)
        {
            if (config.Mode != GameMode.OnlinePvp)
                throw new ArgumentOutOfRangeException(nameof(config.Mode), config.Mode, "OnlineControllerFactory supports only OnlinePvp");

            return new Dictionary<PlayerId, IPlayerController>
            {
                [config.Participants.Local] = _localController,
                [config.Participants.Opponent] = _networkController
            };
        }
    }
}