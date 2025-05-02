using Mutagen.Bethesda.Skyrim;

namespace SkyrimActorValueEditor.Models.ActorValues.Interfaces
{
    public interface IRecordNode<T> where T : ISkyrimMajorRecordGetter
    {
        public T Record { get; }
    }
}