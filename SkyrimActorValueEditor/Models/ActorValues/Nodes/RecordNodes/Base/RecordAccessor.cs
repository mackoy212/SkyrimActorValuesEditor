using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base
{
    public class RecordAccessor<TRecordGetter, TRecordSetter>
        where TRecordGetter : class, ISkyrimMajorRecordGetter
        where TRecordSetter : class, ISkyrimMajorRecord
    {
        private readonly Func<TRecordGetter, float> _getter;

        private readonly Action<TRecordSetter, float> _setter;

        public RecordAccessor(Func<TRecordGetter, float> getter, Action<TRecordSetter, float> setter) 
        {
            _getter = getter;
            _setter = setter;
        }

        public float GetValue(TRecordGetter recordGetter)
        {
            var record = GameReader.GetOriginalOrOverride(recordGetter);
            return _getter(record);
        }

        public void SetValue(TRecordGetter recordGetter, float value)
        {
            var record = GameReader.GetAsMuttable<TRecordGetter, TRecordSetter>(recordGetter);
            _setter(record, value);
        }
    }
}