using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class MagicEffectNodeBuilder : ITreeNodeBuilder<IEffectGetter>
    {
        private readonly ScriptNodeBuilder _scriptNodeBuilder = new();
        private readonly ConditionNodeBuilder<MagicEffectModel> _conditionNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(IEffectGetter effect)
        {
            if (!GameContext.TryResolve(effect.BaseEffect, out var magicEffect))
                yield break;

            if (magicEffect.TargetType is not TargetType.Self)
                yield break;

            foreach (var (actorValue, scriptNode) in _scriptNodeBuilder.TryBuild(magicEffect))
            {
                var magicEffectNode = new MagicEffectNode(magicEffect, effect);
                magicEffectNode.AddNode(scriptNode);
                yield return new(actorValue, magicEffectNode);
            }

            if (magicEffect.Archetype.ActorValue is ActorValue.None)
                yield break;

            switch (magicEffect.Archetype.Type)
            {
                case MagicEffectArchetype.TypeEnum.PeakValueModifier:
                case MagicEffectArchetype.TypeEnum.ValueModifier when magicEffect.Flags.HasFlag(MagicEffect.Flag.Recover):
                    var magicEffectNode = new MagicEffectNode(magicEffect, effect);

                    foreach (var conditionNode in _conditionNodeBuilder.TryBuild(new MagicEffectModel(magicEffect, effect)))
                        magicEffectNode.AddNode(conditionNode);

                    yield return new(
                        magicEffect.Archetype.ActorValue.ToString(),
                        magicEffectNode
                    );
                    break;
            }
        }
    }
}