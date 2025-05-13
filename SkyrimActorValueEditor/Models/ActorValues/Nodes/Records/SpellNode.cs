using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class SpellNode : RecordNode<ISpellGetter>
    {
        public SpellNode(ISpellGetter record) : base(record) { }
    }
}