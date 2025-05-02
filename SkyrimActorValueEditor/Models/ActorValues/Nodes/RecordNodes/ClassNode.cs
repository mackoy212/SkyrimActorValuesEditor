using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class ClassNode : RecordNode<IClassGetter>
    {
        public ClassNode(IClassGetter record) : base(record) { }
    }
}