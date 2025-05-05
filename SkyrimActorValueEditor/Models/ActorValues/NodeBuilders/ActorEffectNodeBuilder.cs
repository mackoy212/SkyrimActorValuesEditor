using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ActorEffectBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly SpellNodeBuilder _spellNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            foreach (var spell in GetActorEffects(npc))
            {
                foreach (var spellNode in _spellNodeBuilder.TryBuild(spell))
                {
                    yield return spellNode;
                }
            }
        }

        private static IEnumerable<ISpellGetter> GetActorEffects(INpcGetter npc)
        {
            if (npc.ActorEffect == null)
                yield break;

            foreach (var spellLink in npc.ActorEffect)
            {
                if (GameContext.TryResolve(spellLink, out var spellRecord)
                    && spellRecord is ISpellGetter spell)
                {
                    yield return spell;
                }
            }
        }
    }
}