using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class MagicEffectNode : RecordNode<IMagicEffectGetter>
    {
        public override float Value
        {
            get => _totalValue;
            set { }
        }

        public IEffectGetter Effect { get; }

        private float _totalValue
        {
            get
            {
                if (Record.Flags.HasFlag(MagicEffect.Flag.Detrimental))
                    return -_value;

                return _value;
            }
        }

        private float _value;

        public MagicEffectNode(IMagicEffectGetter record, IEffectGetter effect)
            : base(record)
        {
            Effect = effect;
            _value = effect.Data?.Magnitude ?? 0.0f;
        }

        public override void UpdateValuesFromDepth()
        {
            bool isEvaluate = Children
                .OfType<ConditionNode>()
                .All(x => x.Value == 1);

            if (isEvaluate)
                _value = Effect.Data?.Magnitude ?? 0.0f;
            else
                _value = 0.0f;

            OnPropertyChanged(nameof(Value));
            Parent?.UpdateValuesFromDepth();
        }
    }
}