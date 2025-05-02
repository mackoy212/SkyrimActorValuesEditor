using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class EffectNode : RecordNode<IMagicEffectGetter>
    {
        public override float Value
        {
            get
            {
                if (!_value.HasValue)
                {
                    _value = Effect.Data?.Magnitude ?? 0.0f;
                    if (IsDetrimental) _value = -_value;
                }

                return _value.Value;
            }
            set { }
        }

        public IEffectGetter Effect { get; }

        public bool IsDetrimental => Record.Flags.HasFlag(MagicEffect.Flag.Detrimental);

        private float? _value;

        public EffectNode(IMagicEffectGetter record, IEffectGetter effect) : base(record)
        {
            Effect = effect;
        }
    }
}