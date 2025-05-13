using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class RaceNode : RecordNode<IRaceGetter>
    {
        public override float Value
        {
            get => _valueGetter(Record);
            set { }
        }

        private readonly Func<IRaceGetter, float> _valueGetter;

        public RaceNode(IRaceGetter record, Func<IRaceGetter, float>? valueGetter = null)
            : base(record)
        {
            _valueGetter = valueGetter ?? (_ => base.Value);
        }
    }
}