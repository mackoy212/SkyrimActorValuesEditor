using Mutagen.Bethesda.Skyrim;

namespace SkyrimActorValueEditor.Models.Common.Conditioned.Interfaces
{
    public interface IConditioned
    {
        IEnumerable<IConditionGetter> Conditions { get; }
    }
}