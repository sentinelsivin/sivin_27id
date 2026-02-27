using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Infrastructure.DataProvider
{
    public class PersistentLocalPlayerIdProvider : ILocalPlayerIdProvider
    {
        private readonly IPersistentData _persistent;

        public PersistentLocalPlayerIdProvider(IPersistentData persistent) => _persistent = persistent;

        public PlayerId GetLocalPlayerId() => _persistent.PlayerData.Id;
    }
}