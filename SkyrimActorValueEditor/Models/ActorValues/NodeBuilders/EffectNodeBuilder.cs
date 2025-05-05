using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class EffectNodeBuilder : ITreeNodeBuilder<IEffectGetter>
    {
        private readonly ScriptNodeBuilder _scriptNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IEffectGetter effect)
        {
            if (TryGetActorValue(effect, out var node))
                yield return node;
        }

        private bool TryGetActorValue(IEffectGetter effect, [MaybeNullWhen(false)] out KeyValuePair<string, TreeNode> effectNode)
        {
            if (GameContext.TryResolve(effect.BaseEffect, out var magicEffect)
                && magicEffect.TargetType is TargetType.Self)
            {               
                if (magicEffect.Archetype.Type is MagicEffectArchetype.TypeEnum.PeakValueModifier)
                {
                    effectNode = new(
                        magicEffect.Archetype.ActorValue.ToString(),
                        new EffectNode(magicEffect, effect)
                    );
                    return true;
                }
            }

            effectNode = default;
            return false;
        }
    }

    #region forFuture

    /*                else if (magicEffect.Archetype.Type is MagicEffectArchetype.TypeEnum.DualValueModifier
                    && !magicEffect.Flags.HasFlag(MagicEffect.Flag.Recover))
                {
                    effectNode = new(
                        magicEffect.Archetype.ActorValue.ToString(),
                        new EffectNode(magicEffect, effect)
                    );
                    effectNode = new(
                        magicEffect.SecondActorValue.ToString(),
                        new EffectNode(magicEffect, effect)
                    );
                    return true;
                }*/

    #endregion

}