using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class EnchantmentNodeBuilder : ITreeNodeBuilder<IObjectEffectGetter>
    {
        private readonly EffectNodeBuilder _effectNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IObjectEffectGetter enchantment)
        {
            var enchantmentNodes = new Dictionary<string, TreeNode>();

            foreach (var effect in enchantment.Effects)
            {
                foreach (var (actorValue, effectNode) in _effectNodeBuilder.TryBuild(effect))
                {
                    if (!enchantmentNodes.TryGetValue(actorValue, out var enchantmentNode))
                    {
                        enchantmentNode = new EnchantmentNode(enchantment);
                        enchantmentNodes[actorValue] = enchantmentNode;
                    }

                    enchantmentNode.AddNode(effectNode);
                }
            }

            return enchantmentNodes;
        }
    }
}