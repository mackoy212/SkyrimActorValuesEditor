using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned.Interfaces;

namespace SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned
{
    public class MagicEffectModel : IConditioned
    {
        public IEnumerable<IConditionGetter> Conditions => _magicEffect.Conditions.Union(_effect.Conditions);

        private readonly IMagicEffectGetter _magicEffect;
        private readonly IEffectGetter _effect;

        public MagicEffectModel(IMagicEffectGetter magicEffect, IEffectGetter effect)
        {
            _magicEffect = magicEffect;
            _effect = effect;
        }
    }
}