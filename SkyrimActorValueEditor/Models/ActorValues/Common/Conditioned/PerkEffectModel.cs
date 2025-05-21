using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned.Interfaces;

namespace SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned
{
    public class PerkEffectModel : IConditioned
    {
        public IEnumerable<IConditionGetter> Conditions => _perkEffect.Conditions
            .First(x => x.RunOnTabIndex == 1) // Self
            .Conditions;

        private readonly IAPerkEffectGetter _perkEffect;

        public PerkEffectModel(IAPerkEffectGetter perkEffect)
        {
            _perkEffect = perkEffect;
        }
    }
}