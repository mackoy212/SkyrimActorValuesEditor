namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Interfaces
{
    public interface IRecordValueAccessor<TGetter, TSetter>
    {
        public float GetValue(TGetter getter);

        public void SetValue(TGetter getter, float value);
    }
}