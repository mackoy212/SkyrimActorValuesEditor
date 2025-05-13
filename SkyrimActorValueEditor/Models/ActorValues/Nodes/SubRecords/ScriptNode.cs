using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords
{
    public class ScriptNode(string name, float value = 0.0f, List<string>? modifiers = null) : TreeNode(name)
    {
        public override float Value { get; set; } = value;

        public List<string>? Modifiers { get; } = modifiers;
    }
}