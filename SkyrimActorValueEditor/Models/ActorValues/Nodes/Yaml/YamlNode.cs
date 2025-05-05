namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Yaml
{
    public class YamlNode
    {
        public string? Name { get; set; }

        public List<YamlNode>? Children { get; set; }
    }
}