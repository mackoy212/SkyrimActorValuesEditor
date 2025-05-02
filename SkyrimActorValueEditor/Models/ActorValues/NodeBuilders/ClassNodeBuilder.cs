using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.Models.ActorValues.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ClassNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            if (GameReader.TryResolve(npc.Class, out var classGetter))
                yield break;

            //
        }
    }
}
