using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.Script;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class ScriptNodeBuilder : ITreeNodeBuilder<IScriptedGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IScriptedGetter record)
        {
            if (record.VirtualMachineAdapter == null)
                yield break;

            foreach (var script in record.VirtualMachineAdapter.Scripts)
            {
                if (!ScriptService.TryGetByName(script.Name, out var scriptInfo))
                    continue;

                var actorValue = script.Properties
                    .OfType<ScriptStringProperty>()
                    .FirstOrDefault(x => x.Name.Equals("ActorValue", StringComparison.OrdinalIgnoreCase));

                if (actorValue != null)
                    yield return new(actorValue.Data, new ScriptNode(script.Name));
            }
        }
    }
}