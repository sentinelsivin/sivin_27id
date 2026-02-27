using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Infrastructure.DataProvider
{
    public interface ILocalPlayerIdProvider
    {
        PlayerId GetLocalPlayerId();
    }
}