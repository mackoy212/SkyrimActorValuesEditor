using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class EffectNodeBuilder : ITreeNodeBuilder<IEffectGetter>
    {
        private readonly ScriptNodeBuilder _scriptNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IEffectGetter effect)
        {
            if (!GameContext.TryResolve(effect.BaseEffect, out var magicEffect))
                yield break;

            if (magicEffect.TargetType is not TargetType.Self)
                yield break;

            if (magicEffect.Archetype.ActorValue is ActorValue.None)
                yield break;

            switch (magicEffect.Archetype.Type)
            {
                case MagicEffectArchetype.TypeEnum.PeakValueModifier:
                case MagicEffectArchetype.TypeEnum.ValueModifier when magicEffect.Flags.HasFlag(MagicEffect.Flag.Recover):
                    yield return new(
                        magicEffect.Archetype.ActorValue.ToString(),
                        new EffectNode(magicEffect, effect)
                    );
                    break;
            }
        }
    }
}