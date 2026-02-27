using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Controllers
{
    public class VsAiControllerFactory : IControllerFactory
    {
        private readonly IPlayerController _localController;
        private readonly IPlayerController _aiController;

        public VsAiControllerFactory(IPlayerController localController, IPlayerController aiController)
        {
            _localController = localController;
            _aiController = aiController;
        }

        public IReadOnlyDictionary<PlayerId, IPlayerController> CreateControllers(GameStartConfig config)
        {
            if (config.Mode != GameMode.VsAi)
                throw new ArgumentOutOfRangeException(nameof(config.Mode), config.Mode, "VsAiControllerFactory supports only VsAi");

            return new Dictionary<PlayerId, IPlayerController>
            {
                [config.Participants.Local] = _localController,
                [config.Participants.Opponent] = _aiController
            };
        }
    }
}