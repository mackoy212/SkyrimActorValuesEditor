namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base
{
    public class RecordAccessor<TRecordGetter, TRecordSetter, TValue>
    {
        public Func<TRecordGetter, TValue> Getter { get; }

        public Action<TRecordSetter, TValue> Setter { get; }

        public RecordAccessor(Func<TRecordGetter, TValue> getter, Action<TRecordSetter, TValue> setter) 
        {
            Getter = getter;
            Setter = setter;
        }
    }
}