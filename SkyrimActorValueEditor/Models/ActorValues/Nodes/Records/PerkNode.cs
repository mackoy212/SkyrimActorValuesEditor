using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class PerkNode : RecordNode<IPerkGetter>
    {
        public PerkNode(IPerkGetter record) : base(record) { }
    }
}