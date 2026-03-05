using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match.Module;

namespace CodeBase.Domain.Match
{
    public class MatchFactory
    {
        private readonly IMatchRules _rules;

        public MatchFactory(IMatchRules rules) => _rules = rules;

        public Match Create(IReadOnlyList<PlayerId> players, PlayerId firstPlayer) => 
            new(new MatchState(players, firstPlayer), _rules);
    }
}