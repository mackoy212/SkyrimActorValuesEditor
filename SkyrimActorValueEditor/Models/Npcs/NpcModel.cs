using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services.GameData;
using SkyrimActorValueEditor.ViewModels.Utils;

namespace SkyrimActorValueEditor.Models.Npcs
{
    public class NpcModel
    {
        public INpcGetter NPC => _npc;
        public string FormKey => _npc.FormKey.ToString();
        public string? EditorID => _npc.EditorID;
        public string? Name => RecordUtil.GetName(_npc);

        private readonly INpcGetter _npc;

        public NpcModel(INpcGetter npc)
        {
            _npc = npc;
        }

        public override string ToString() => $"{FormKey} {EditorID} {Name}";
    }
}