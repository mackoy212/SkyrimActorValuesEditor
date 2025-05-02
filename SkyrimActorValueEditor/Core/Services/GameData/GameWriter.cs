using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using System.IO;

namespace SkyrimActorValueEditor.Core.Services.GameData
{
    public static class GameWriter
    {
        private static readonly SkyrimMod _outputMod = GamePlugin.OutputMod;
        private static readonly string _outputModPath = Path.Combine(PathService.PathSkyrimData, _outputMod.ModKey.FileName);

        private static readonly List<ISkyrimMajorRecordGetter> _overrideRecords = new();

        public static void AddToOverride(ISkyrimMajorRecord record)
        {
            if (!_overrideRecords.Contains(record))
            {
                _overrideRecords.Add(record);
            }
        }

        public static void SaveChanges()
        {
            foreach (var record in _overrideRecords)
            {
                if (record is Npc npc)
                {
                    _outputMod.Npcs.GetOrAddAsOverride(npc);
                }
            }

            SaveMod();
        }

        private static void SaveMod()
        {
            _outputMod.WriteToBinary(_outputModPath);
        }
    }
}