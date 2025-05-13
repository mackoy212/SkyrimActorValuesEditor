using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords
{
    public class PerkEntryPointModifyValueNode : TreeNode
    {
        public override float Value
        {
            get => _value;
            set { }
        }

        public IPerkEntryPointModifyValueGetter EntryPoint { get; }

        private float _value;

        public PerkEntryPointModifyValueNode(IPerkEntryPointModifyValueGetter perkEntryPointEffect)
            : base(GetPerkEntryPointName(perkEntryPointEffect))
        {
            EntryPoint = perkEntryPointEffect;
            _value = perkEntryPointEffect.Value ?? 0.0f;
        }

        public override void UpdateValuesFromDepth()
        {
            bool isEvaluate = Children
                .OfType<ConditionNode>()
                .All(x => x.Value == 1);

            if (isEvaluate)
                _value = EntryPoint.Value ?? 0.0f;
            else
                _value = 0.0f;

            OnPropertyChanged(nameof(Value));
            Parent?.UpdateValuesFromDepth();
        }

        private static string GetPerkEntryPointName(IPerkEntryPointModifyValueGetter perkEntryPointEffect)
        {
            return perkEntryPointEffect.EntryPoint.ToString();
        }
    }
}