using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class ArmorNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly EnchantmentNodeBuilder _enchantmentNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            var armorNodes = new Dictionary<(string actorValue, FormKey armorKey), TreeNode>();

            foreach (var armor in GetArmorsWithArmorRating(npc))
            {
                if (TryGetEnchantment(armor, out var enchantment))
                {
                    foreach (var (actorValue, enchantmentNode) in _enchantmentNodeBuilder.TryBuild(enchantment))
                    {
                        var key = (actorValue, armor.FormKey);

                        if (!armorNodes.TryGetValue(key, out var armorNode))
                        {
                            armorNode = new ArmorNode(armor);
                            armorNodes[key] = armorNode;
                        }

                        armorNode.AddNode(enchantmentNode);

                    }
                }

                yield return new(
                    "DamageResist",
                    new ArmorNode(armor, r => r.ArmorRating)
                );
            }

            foreach (var kvp in armorNodes)
                yield return new(kvp.Key.actorValue, kvp.Value);

        }

        private static IEnumerable<IArmorGetter> GetArmorsWithArmorRating(INpcGetter npc)
        {
            if (!GameContext.TryResolve(npc.DefaultOutfit, out var outfit) || outfit.Items == null)
                yield break;

            foreach (var outfitTargetLink in outfit.Items)
            {
                if (GameContext.TryResolve(outfitTargetLink, out var outfitTarget)
                    && outfitTarget is IArmorGetter armor
                    && armor.ArmorRating != 0.0f)
                {
                    yield return armor;
                }
            }

            if (npc.Items != null)
            {
                foreach (var containerEntry in npc.Items)
                {
                    if (GameContext.TryResolve(containerEntry.Item.Item, out var item)
                        && item is IArmorGetter armor
                        && armor.ArmorRating != 0.0f)
                    {
                        yield return armor;
                    }
                }
            }
        }

        private static bool TryGetEnchantment(IArmorGetter armor, [MaybeNullWhen(false)] out IObjectEffectGetter enchantment)
        {
            enchantment = GameContext.TryResolve(armor.ObjectEffect, out var effectRecord)
                ? effectRecord : null;

            return enchantment != null;
        }
    }
}