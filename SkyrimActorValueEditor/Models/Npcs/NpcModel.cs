using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Extensions;

namespace SkyrimActorValueEditor.Models.Npcs
{
    public class NpcModel
    {
        public INpcGetter NPC => _npc;
        public string FormKey => _npc.FormKey.ToString();
        public string? EditorID => _npc.EditorID;
        public string? Name => _npc.GetName();

        private readonly INpcGetter _npc;

        public NpcModel(INpcGetter npc)
        {
            _npc = npc;
        }

        public override string ToString() => $"{FormKey} {EditorID} {Name}";
    }
}