using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SeparateNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Yaml
{
    public class YamlNodeBuilder
    {
        private List<TreeNode> _categoryNodes = new();
        private Dictionary<string, ActorValueNode> _actorValuesNodesDictionary = new();

        public (List<TreeNode>, Dictionary<string, ActorValueNode>) LoadActorValues()
        {
            foreach (var yamlNode in CreateYamlTree("ActorValues.yaml"))
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
                var categoryNode = new CategoryNode(yamlNode.Name!);

                foreach (var childYamlNode in yamlNode.Children)
                {
                    var childNode = Convert(childYamlNode);
                    categoryNode.AddNode(childNode);
                }

                return categoryNode;
            }
            else
            {
                var actorValueNode = new ActorValueNode(yamlNode.Name!);
                _actorValuesNodesDictionary.Add(actorValueNode.Name, actorValueNode);

                return actorValueNode;
            }
        }

        private static List<YamlNode> CreateYamlTree(string path)
        {
            return YamlService.Load<List<YamlNode>>(path);
        }
    }
}