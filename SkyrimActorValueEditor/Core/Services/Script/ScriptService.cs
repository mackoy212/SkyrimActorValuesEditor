using SkyrimActorValueEditor.Core.Services.Yaml;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SkyrimActorValueEditor.Core.Services.Script
{
    public static class ScriptService
    {
        private static readonly List<ScriptInfo> _scripts = new();

        static ScriptService()
        {
            var scriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Scripts");

            foreach (var script in Directory.GetFiles(scriptsPath))
            {
                var scriptInfos = YamlService.LoadFromFullPath<List<ScriptInfo>>(script);
                _scripts.AddRange(scriptInfos);
            }
        }

        public static bool TryGetByName(string scriptName, [MaybeNullWhen(false)] out ScriptInfo script)
        {
            script = _scripts
                .FirstOrDefault(x => x.Name.Equals(scriptName));

            return script != null;
        }
    }
}