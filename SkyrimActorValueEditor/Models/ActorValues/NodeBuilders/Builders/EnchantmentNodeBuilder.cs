using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class EnchantmentNodeBuilder : ITreeNodeBuilder<IObjectEffectGetter>
    {
        private readonly MagicEffectNodeBuilder _magicEffectNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IObjectEffectGetter enchantment)
        {
            var enchantmentNodes = new Dictionary<string, TreeNode>();

            foreach (var effect in enchantment.Effects)
            {
                foreach (var (actorValue, effectNode) in _magicEffectNodeBuilder.TryBuild(effect))
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