using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.Models.ActorValues.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;
using System.Diagnostics.CodeAnalysis;
using static Mutagen.Bethesda.Skyrim.Package;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ArmorNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly EnchantmentNodeBuilder _enchantmentNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            var armorNodes = new Dictionary<string, TreeNode>();

            foreach (var armor in GetArmors(npc))
            {
                if (TryGetEnchantment(armor, out var enchantment))
                {
                    foreach (var (actorValue, enchantmentNode) in _enchantmentNodeBuilder.TryBuild(enchantment))
                    {
                        if (!armorNodes.TryGetValue(actorValue, out var armorNode))
                        {
                            armorNode = new ArmorNode(armor);
                            armorNodes[actorValue] = armorNode;
                        }

                        armorNode.AddNode(enchantmentNode);
                    }
                }

                armorNodes["DamageResist"] = new ArmorNode(armor, r => r.ArmorRating);
            }

            return armorNodes;
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
            enchantment = GameReader.TryResolve(armor.ObjectEffect, out var effectRecord)
                ? effectRecord as IObjectEffectGetter
                : null;

            return enchantment != null;
        }
    }
}