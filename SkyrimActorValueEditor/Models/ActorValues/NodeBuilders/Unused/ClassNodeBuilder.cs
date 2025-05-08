using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Unused
{
    public class ClassNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            if (GameContext.TryResolve(npc.Class, out var classGetter))
                yield break;

            //
        }
    }
}
