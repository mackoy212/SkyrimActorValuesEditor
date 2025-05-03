using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using System.IO;

namespace SkyrimActorValueEditor.Core.Services.GameData
{
    public static class GameWriter
    {
        private static readonly SkyrimMod _outputMod = GamePlugin.OutputMod;

        private static readonly string _outputModPath = Path.Combine(PathService.PathSkyrimData, _outputMod.ModKey.FileName);

        public static T GetOrAddAsOverride<T>(T record)
            where T : class, ISkyrimMajorRecord
        {
            return _outputMod.GetTopLevelGroup<T>()
                .GetOrAddAsOverride(record);
        }

        public static void SaveChanges()
        {
            _outputMod.WriteToBinary(_outputModPath);
        }
    }
}