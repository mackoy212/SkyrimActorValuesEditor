using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class PerkNode : RecordNode<IPerkGetter>
    {
        public PerkNode(IPerkGetter record) : base(record) { }
    }
}