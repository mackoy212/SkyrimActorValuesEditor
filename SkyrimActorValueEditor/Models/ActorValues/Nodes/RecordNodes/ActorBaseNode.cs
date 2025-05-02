using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes
{
    public class ActorBaseNode : RecordNode<INpcGetter>
    {
        public override float Value
        {
            get => _accessor.Getter(_npc ?? Record);
            set
            {
                _accessor.Setter(_muttableRecord, value);
                GameWriter.AddToOverride(_muttableRecord);
                OnPropertyChanged();
            }
        }

        public override bool IsEditable => !Record.Configuration.Flags.HasFlag(NpcConfiguration.Flag.AutoCalcStats);

        private INpc _muttableRecord => _npc ??= Record.DeepCopy();

        private INpc? _npc;

        private readonly RecordAccessor<INpcGetter, INpc, float> _accessor;

        public ActorBaseNode(string name, INpcGetter record, Func<INpcGetter, float> valueGetter, Action<INpc, float> valueSetter) 
            : base(name, record)
        {
            _accessor = new (valueGetter, valueSetter);
        }
    }
}