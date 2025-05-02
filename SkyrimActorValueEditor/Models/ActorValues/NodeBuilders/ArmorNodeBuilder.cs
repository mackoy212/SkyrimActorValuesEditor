using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.Models.ActorValues.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ArmorNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly EnchantmentNodeBuilder _enchantmentNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            foreach (var armor in GetArmors(npc))
            {
                if (TryGetEnchantment(armor, out var enchantment))
                {
                    foreach (var (actorValue, enchantmentNode) in _enchantmentNodeBuilder.TryBuild(enchantment))
                    {
                        var armorNode = new ArmorNode(armor);
                        armorNode.AddNode(enchantmentNode);
                        yield return new(actorValue, armorNode);
                    }
                }

                yield return new("DamageResist", new ArmorNode(armor, r => r.ArmorRating));
            }
        }

        private static IEnumerable<IArmorGetter> GetArmors(INpcGetter npc)
        {
            if (!GameReader.TryResolve(npc.DefaultOutfit, out var outfit) || outfit.Items == null)
                yield break;

            foreach (var outfitTargetLink in outfit.Items)
            {
                if (GameReader.TryResolve(outfitTargetLink, out var outfitTarget)
                    && outfitTarget is IArmorGetter armor)
                {
                    yield return armor;
                }
            }
        }

        private static bool TryGetEnchantment(IArmorGetter armor, [MaybeNullWhen(false)] out IObjectEffectGetter enchantment)
        {
            if (GameReader.TryResolve(armor.ObjectEffect, out var effectRecord)
                && effectRecord is IObjectEffectGetter ench)
            {
                enchantment = ench;
                return true;
            }

            enchantment = null;
            return false;
        }
    }
}