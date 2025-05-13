namespace SkyrimActorValueEditor.Core.Services.Script
{
    public class ScriptInfo
    {
        public string? Name { get; set; }

        public List<string>? Value { get; set; }

        public List<string>? Properties { get; set; }

        public Dictionary<string, string>? Modifiers { get; set; }
    }
}