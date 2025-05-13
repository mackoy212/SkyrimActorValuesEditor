using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class EnchantmentNode : RecordNode<IObjectEffectGetter>
    {
        public EnchantmentNode(IObjectEffectGetter record) : base(record) { }
    }
}