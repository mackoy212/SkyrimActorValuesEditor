using Mutagen.Bethesda.Skyrim;
using System.IO;

namespace SkyrimActorValueEditor.Core.Services.GameData
{
    public static class GamePlugin
    {
        public static SkyrimMod OutputMod { get; } = GetOrCreateMod("SAVE_ActorsChanges.esp");

        private static SkyrimMod GetOrCreateMod(string modName)
        {
            var path = Path.Combine(PathService.PathSkyrimData, modName);

            if (File.Exists(path))
            {
                return SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);
            }

            return new SkyrimMod(modName, SkyrimRelease.SkyrimSE, 1.7f);
        }
    }
}