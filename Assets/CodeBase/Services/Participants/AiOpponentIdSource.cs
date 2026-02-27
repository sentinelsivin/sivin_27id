using CodeBase.Data.PlayerDataComponents;


namespace CodeBase.Services.Participants
{
    public sealed class AiOpponentIdSource : IOpponentIdSource
    {
        public PlayerId GetOpponentId() => new PlayerId(999);
    }
}