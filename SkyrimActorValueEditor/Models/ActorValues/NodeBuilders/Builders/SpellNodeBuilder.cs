using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class SpellNodeBuilder : ITreeNodeBuilder<ISpellGetter>
    {
        private readonly MagicEffectNodeBuilder _effectNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(ISpellGetter spell)
        {
            var spellNodes = new Dictionary<string, TreeNode>();

            foreach (var effect in spell.Effects)
            {
                foreach (var (actorValue, effectNode) in _effectNodeBuilder.TryBuild(effect))
                {
                    if (!spellNodes.TryGetValue(actorValue, out var spellNode))
                    {
                        spellNode = new SpellNode(spell);
                        spellNodes[actorValue] = spellNode;
                    }

                    spellNode.AddNode(effectNode);
                }
            }

            return spellNodes;
        }
    }
}