using SkyrimActorValueEditor.Core.Extensions;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords;
using SkyrimActorValueEditor.Models.Common.Conditioned.Interfaces;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class ConditionNodeBuilder<TConditioned>
        where TConditioned : class, IConditioned
    {
        public IEnumerable<ConditionNode> TryBuild(TConditioned source)
        {
            foreach (var condition in source.Conditions)
                yield return new ConditionNode(condition.ToDisplayString());
        }
    }
}