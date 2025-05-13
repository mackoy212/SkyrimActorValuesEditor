using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Extensions;
using SkyrimActorValueEditor.Core.Services;
using System.Collections.ObjectModel;

namespace SkyrimActorValueEditor.Models.Npcs
{
    public class NpcModel
    {
        public INpcGetter NPC => _npc;
        public string FormKey => _npc.FormKey.ToString();
        public string? EditorID => _npc.EditorID;
        public string? Name => _npc.GetName();

        public bool HasTemplates => !_npc.Template.IsNull;

        public ObservableCollection<NpcModel>? Templates
        {
            get
            {
                if (!HasTemplates)
                    return null;

                if (_templates == null)
                {
                    _templates = new();

                    if (GameContext.TryResolve(_npc.Template, out var npcSpawn))
                    {
                        foreach (var template in GetTemplates(npcSpawn))
                            _templates.Add(template);
                    }
                }

                return _templates;
            }
        }

        private readonly INpcGetter _npc;
        private ObservableCollection<NpcModel>? _templates;

        public NpcModel(INpcGetter npc)
        {
            _npc = npc;
        }

        public override string ToString() => $"{FormKey} {EditorID} {Name}";

        private static IEnumerable<NpcModel> GetTemplates(INpcSpawnGetter npcSpawn)
        {
            if (npcSpawn is INpcGetter npc
                && NpcService.TryGetByFormKey(npc.FormKey, out var npcModel))
            {
                yield return npcModel;
                yield break;
            }

            if (npcSpawn is ILeveledNpcGetter list)
            {
                if (list.Entries == null)
                    yield break;

                foreach (var listEntry in list.Entries)
                {
                    if (listEntry.Data == null)
                        continue;

                    if (!GameContext.TryResolve(listEntry.Data.Reference, out var localNpcSpawn))
                        continue;

                    foreach (var template in GetTemplates(localNpcSpawn))
                        yield return template;
                }
            }
        }
    }
}