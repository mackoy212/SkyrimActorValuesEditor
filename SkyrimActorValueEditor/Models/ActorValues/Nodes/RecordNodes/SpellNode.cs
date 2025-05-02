using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class SpellNode : RecordNode<ISpellGetter>
    {
        public SpellNode(ISpellGetter record) : base(record) { }
    }
}