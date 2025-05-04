using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class ArmorNode : RecordNode<IArmorGetter>
    {
        public override float Value
        {
            get => _accessor(Record);
            set { }
        }

        private readonly Func<IArmorGetter, float> _accessor;

        public ArmorNode(IArmorGetter record, Func<IArmorGetter, float>? accessor = null) : base(record)
        {
            _accessor = accessor ?? (_ => base.Value);
        }
    }
}