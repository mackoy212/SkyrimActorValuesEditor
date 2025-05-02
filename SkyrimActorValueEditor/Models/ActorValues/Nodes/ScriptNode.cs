using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes
{
    public class ScriptNode : TreeNode
    {
        public override float Value
        {
            get => _value;
            set { }
        }

        private float _value;

        public ScriptNode(string name, float value) : base(name)
        {
            _value = value;
        }
    }
}