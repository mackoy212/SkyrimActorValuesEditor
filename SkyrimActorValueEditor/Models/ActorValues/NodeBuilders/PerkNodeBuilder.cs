using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.Models.ActorValues.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class PerkNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly SpellNodeBuilder _spellNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            var perkNodes = new Dictionary<string, TreeNode>();

            foreach (var perk in GetPerks(npc))
            {
                foreach (var spell in GetAbilities(perk))
                {
                    foreach (var (actorValue, spellNode) in _spellNodeBuilder.TryBuild(spell))
                    {
                        if (!perkNodes.TryGetValue(actorValue, out var perkNode))
                        {
                            perkNode = new PerkNode(perk);
                            perkNodes[actorValue] = perkNode;
                        }

                        perkNode.AddNode(spellNode);
                    }
                }
            }

            return perkNodes;
        }

        private static IEnumerable<ISpellGetter> GetAbilities(IPerkGetter perk)
        {
            foreach (var effect in perk.Effects)
            {
                if (effect is IPerkAbilityEffectGetter perkAbilityEffect
                    && GameReader.TryResolve(perkAbilityEffect.Ability, out var spell))
                {
                    yield return spell;
                }
            }
        }

        private static IEnumerable<IPerkGetter> GetPerks(INpcGetter npc)
        {
            if (npc.Perks == null)
                yield break;

            foreach (var perkPlacement in npc.Perks)
            {
                if (GameReader.TryResolve(perkPlacement.Perk, out var perk))
                {
                    yield return perk;
                }
            }
        }
    }
}
