using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class YamlService
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

        public static T Load<T>(string relativePath)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", relativePath);

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