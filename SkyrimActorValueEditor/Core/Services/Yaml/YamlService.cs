using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SkyrimActorValueEditor.Core.Services.Yaml
{
    public static class YamlService
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

        public static T Load<T>(StreamReader reader)
        {
            return _deserializer.Deserialize<T>(reader);
        }

        public static T Load<T>(string relativePath)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", relativePath);
            return LoadFromFullPath<T>(fullPath);
        }

        public static T LoadFromFullPath<T>(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Yaml not found: {fullPath}");

            try
            {
                var content = File.ReadAllText(fullPath);
                return _deserializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load Yaml file: {fullPath}", ex);
            }
        }
    }
}