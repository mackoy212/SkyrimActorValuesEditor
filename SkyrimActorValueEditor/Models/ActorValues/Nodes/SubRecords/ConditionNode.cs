using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords
{
    public class ConditionNode(string name) : TreeNode(name)
    {
        public override float Value
        {
            get => _value;
            set
            {
                _value = value != 0.0f ? 1.0f : 0.0f;
                UpdateValuesFromDepth();
            }
        }

        public override bool IsEditable => true;

        private float _value = 1.0f;
    }
}