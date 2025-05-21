using SkyrimActorValueEditor.Core.Extensions;
using SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords;

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