using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class ArmorNode : RecordNode<IArmorGetter>
    {
        public override float Value
        {
            get => _valueGetter(Record);
            set { }
        }

        private readonly Func<IArmorGetter, float> _valueGetter;

        public ArmorNode(IArmorGetter record, Func<IArmorGetter, float>? valueGetter = null) : base(record)
        {
            _valueGetter = valueGetter ?? (_ => base.Value);
        }
    }
}