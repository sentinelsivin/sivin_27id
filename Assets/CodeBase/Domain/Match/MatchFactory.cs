using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain.Match
{
    public class MatchFactory
    {
        private readonly IMatchRules _rules;

        public MatchFactory(IMatchRules rules) => _rules = rules;

        public Match Create(IReadOnlyList<PlayerId> players)
        {
            var first = players[UnityEngine.Random.Range(0, players.Count)];
            return new Match(players, first, _rules);
        }
    }
}