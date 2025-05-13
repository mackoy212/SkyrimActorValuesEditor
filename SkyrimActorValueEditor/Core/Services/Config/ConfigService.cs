using SkyrimActorValueEditor.Core.Services.Yaml;

namespace SkyrimActorValueEditor.Core.Services.Config
{
    public class AppConfig
    {
        public bool Debug { get; set; } = false;
    }

    public static class ConfigService
    {
        private static AppConfig? _config;

        public static AppConfig Get()
        {
            if (_config == null)
                _config = YamlService.Load<AppConfig>("Settings.yaml");

            return _config;
        }
    }
}