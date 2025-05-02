using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class EnchantmentNode : RecordNode<IObjectEffectGetter>
    {
        public EnchantmentNode(IObjectEffectGetter record) : base(record) { }
    }
}