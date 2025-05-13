using SkyrimActorValueEditor.Core.Services.Yaml;
using System.IO;
using System.Reflection;

namespace SkyrimActorValueEditor.Core.Services.Config
{
    public static class SyncfusionKeyGetter
    {
        private class KeyGetter
        {
            public string? SyncfusionLicenseKey { get; set; }
        }

        public static string GetKey()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SkyrimActorValueEditor.Resources.Secrets.yaml";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);

            var config = YamlService.Load<KeyGetter>(reader);

            return config.SyncfusionLicenseKey!;
        }
    }
}