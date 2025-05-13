using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class ClassNode : RecordNode<IClassGetter>
    {
        public ClassNode(IClassGetter record) : base(record) { }
    }
}