using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.Records
{
    public class ActorBaseNode : RecordNode<INpcGetter>
    {
        public override float Value
        {
            get => _accessor.GetValue(Record);
            set => _accessor.SetValue(Record, value);
        }

        public override bool IsEditable => !Record.Configuration.Flags.HasFlag(NpcConfiguration.Flag.AutoCalcStats);

        private readonly MajorRecordValueAccessor<INpcGetter, Npc> _accessor;

        public ActorBaseNode(string name, INpcGetter record, MajorRecordValueAccessor<INpcGetter, Npc> accessor) 
            : base(name, record)
        {
            _accessor = accessor;
        }
    }
}