using Mutagen.Bethesda;
using Mutagen.Bethesda.Installs;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class PathService
    {
        public static string PathSkyrimData { get; }

        static PathService()
        {
            if (!GameLocations.TryGetDataFolder(GameRelease.SkyrimSE, out var pathSkyrimData))
            {
                throw new Exception("Skyrim SE is not installed.");
            }

            PathSkyrimData = pathSkyrimData;
        }
    }
}