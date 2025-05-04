using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Interfaces;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base
{
    public class MajorRecordValueAccessor<TReadOnly, TEditable> : IRecordValueAccessor<TReadOnly, TEditable>
        where TReadOnly : class, ISkyrimMajorRecordGetter
        where TEditable : class, ISkyrimMajorRecord, TReadOnly
    {
        private readonly Func<TReadOnly, float> _getter;
        private readonly Action<TEditable, float> _setter;

        public MajorRecordValueAccessor(Func<TReadOnly, float> getter, Action<TEditable, float> setter) 
        {
            _getter = getter;
            _setter = setter;
        }

        public float GetValue(TReadOnly recordGetter)
        {
            var record = GameContext.GetOriginalOrOverride(recordGetter);
            return _getter(record);
        }

        public void SetValue(TReadOnly recordGetter, float value)
        {
            var record = GameContext.GetAsOverride<TReadOnly, TEditable>(recordGetter);
            _setter(record, value);
        }
    }
}