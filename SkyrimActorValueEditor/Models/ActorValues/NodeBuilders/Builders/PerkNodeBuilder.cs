using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.SubRecords;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class PerkNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly SpellNodeBuilder _spellNodeBuilder = new();
        private readonly ConditionNodeBuilder<PerkEffectModel> _conditionNodeBuilder = new();

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

                foreach (var armorRatingEntryPointNode in GetModArmorRatingEntryPointsNodes(perk))
                {
                    if (!perkNodes.TryGetValue("DamageResist", out var perkNode))
                    {
                        perkNode = new PerkNode(perk);
                        perkNodes["DamageResist"] = perkNode;
                    }

                    perkNode.AddNode(armorRatingEntryPointNode);
                }
            }

            return perkNodes;
        }

        private IEnumerable<TreeNode> GetModArmorRatingEntryPointsNodes(IPerkGetter perk)
        {
            foreach (var effect in perk.Effects)
            {
                if (effect is IAPerkEntryPointEffect perkEntryPointEffect
                    && perkEntryPointEffect is IPerkEntryPointModifyValueGetter perkEntryPointModifyValue
                    && perkEntryPointModifyValue.EntryPoint is APerkEntryPointEffect.EntryType.ModArmorRating
                    && perkEntryPointModifyValue.Modification is PerkEntryPointModifyValue.ModificationType.Add)
                {
                    var perkEntryPoint = new PerkEntryPointModifyValueNode(perkEntryPointModifyValue);

                    foreach (var conditionNode in _conditionNodeBuilder.TryBuild(new PerkEffectModel(effect)))
                        perkEntryPoint.AddNode(conditionNode);

                    yield return perkEntryPoint;
                }
            }
        }

        private static IEnumerable<ISpellGetter> GetAbilities(IPerkGetter perk)
        {
            foreach (var effect in perk.Effects)
            {
                if (effect is IPerkAbilityEffectGetter perkAbilityEffect
                    && GameContext.TryResolve(perkAbilityEffect.Ability, out var spell))
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
                if (GameContext.TryResolve(perkPlacement.Perk, out var perk))
                {
                    yield return perk;
                }
            }
        }
    }
}
