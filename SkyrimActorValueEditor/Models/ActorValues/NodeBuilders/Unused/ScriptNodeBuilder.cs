using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SeparateNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Unused
{
    public class ScriptNodeBuilder : ITreeNodeBuilder<IScriptedGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IScriptedGetter record)
        {
            if (record.VirtualMachineAdapter == null)
                yield break;

            foreach (var script in record.VirtualMachineAdapter.Scripts)
            {
                var actorValue = script.Properties
                    .OfType<ScriptStringProperty>()
                    .FirstOrDefault(x => x.Name == "ActorValue");

                if (actorValue != null)
                {
                    yield return new(actorValue.Data, new ScriptNode(script.Name, 0.0f));
                }
            }
        }
    }
}