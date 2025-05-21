using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Core.Services.Yaml;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Separate;
using SkyrimActorValueEditor.Models.Npcs;

namespace SkyrimActorValueEditor.Models.ActorValues
{
    public static class ActorValueNodeBuilder
    {
        private static readonly Dictionary<string, ITreeNodeBuilder<INpcGetter>> _nodeBuilders = new()
        {
            { "Actor Effects", new ActorEffectNodeBuilder() },
            { "Armor", new ArmorNodeBuilder() },
            { "ActorBase", new ActorBaseNodeBuilder() },
            { "Race", new RaceNodeBuilder() },
            { "Perks", new PerkNodeBuilder() }
        };

        private static readonly List<TreeNode> _categoryNodes;
        private static readonly Dictionary<string, ActorValueNode> _actorValuesNodesDictionary;

        static ActorValueNodeBuilder()
        {
            var yamlBuilder = new YamlNodeBuilder();
            var (categoryNodes, actorValuesNodesDictionary) = yamlBuilder.LoadActorValues();

            _categoryNodes = categoryNodes;
            _actorValuesNodesDictionary = actorValuesNodesDictionary;
        }

        public static IEnumerable<TreeNode> LoadActorValues() => _categoryNodes;

        public static void RebuildActorValues(NpcModel npcModel)
        {
            foreach (var treeNode in _actorValuesNodesDictionary.Values)
                treeNode.ClearNode();

            foreach (var (key, treeNode) in GetActorValues(NpcService.GetNpcWithTemplates(npcModel)))
                _actorValuesNodesDictionary[key].AddNode(treeNode);

            foreach (var treeNode in _categoryNodes)
                treeNode.UpdateValuesToDepth();
        }

        private static IEnumerable<KeyValuePair<string, TreeNode>> GetActorValues(INpcGetter npc)
        {
            foreach (var (sourceName, nodeBuilder) in _nodeBuilders)
            {
                foreach (var (actorValue, buildedNode) in nodeBuilder.TryBuild(npc))
                {
                    if (!_actorValuesNodesDictionary.TryGetValue(actorValue, out var actorValueNode))
                        continue;

                    if (!actorValueNode.TryGetNodeByName(sourceName, out var sourceNode))
                        sourceNode = new SourceNode(sourceName);

                    sourceNode.AddNode(buildedNode);

                    if (!actorValueNode.HasNode(sourceNode))
                        yield return new(actorValue, sourceNode);
                }
            }
        }
    }
}