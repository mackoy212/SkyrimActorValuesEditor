using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SeparateNodes;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SkyrimActorValueEditor.Models.ActorValues.Yaml
{
    public class YamlNodeBuilder
    {
        private List<TreeNode> _categoryNodes = new();
        private Dictionary<string, ActorValueNode> _actorValuesNodesDictionary = new();

        public (List<TreeNode>, Dictionary<string, ActorValueNode>) LoadActorValues()
        {
            var yamlNodes = CreateYamlTree("SkyrimActorValueEditor.Resources.ActorValues.yaml");

            foreach (var yamlNode in yamlNodes)
            {
                var treeNode = Convert(yamlNode);
                _categoryNodes.Add(treeNode);
            }

            return (_categoryNodes, _actorValuesNodesDictionary);
        }

        private TreeNode Convert(YamlNode yamlNode)
        {
            if (yamlNode.Children?.Count > 0)
            {
                var node = new CategoryNode(yamlNode.Name!);

                foreach (var child in yamlNode.Children)
                {
                    var treeNode = Convert(child);
                    node.AddNode(treeNode);
                }

                return node;
            }
            else
            {
                var node = new ActorValueNode(yamlNode.Name!);
                _actorValuesNodesDictionary.Add(node.Name, node);

                return node;
            }
        }

        private static List<YamlNode> CreateYamlTree(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(path);
            using var reader = new StreamReader(stream!);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

            var yaml = reader.ReadToEnd();

            return deserializer.Deserialize<List<YamlNode>>(yaml);
        }
    }
}