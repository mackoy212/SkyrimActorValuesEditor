using Mutagen.Bethesda;
using Mutagen.Bethesda.Installs;
using System.IO;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class PathService
    {
        public static string PathProgram { get; }
        public static string PathSkyrimData { get; }
        public static string PathSkyrimScripsSource { get; }

        static PathService()
        {
            if (!GameLocations.TryGetDataFolder(GameRelease.SkyrimSE, out var pathSkyrimData))
            {
                throw new Exception("Skyrim SE is not installed.");
            }

            PathProgram = AppDomain.CurrentDomain.BaseDirectory;
            PathSkyrimData = pathSkyrimData;
            PathSkyrimScripsSource = Path.Combine(PathSkyrimData, "Scripts", "Source");
        }
    }
}