using Mutagen.Bethesda.Skyrim;

namespace SkyrimActorValueEditor.Models.ActorValues.Common.Conditioned.Interfaces
{
    public interface IConditioned
    {
        IEnumerable<IConditionGetter> Conditions { get; }
    }
}