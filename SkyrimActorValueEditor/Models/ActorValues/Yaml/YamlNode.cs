namespace SkyrimActorValueEditor.Models.ActorValues.Yaml
{
    public class YamlNode
    {
        public string? Name { get; set; }

        public List<YamlNode>? Children { get; set; }
    }
}