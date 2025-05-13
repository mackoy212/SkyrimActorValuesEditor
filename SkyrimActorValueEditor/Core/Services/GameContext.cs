using Mutagen.Bethesda;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Installs;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class GameContext
    {
        private static readonly IGameEnvironment<ISkyrimMod, ISkyrimModGetter> _environment;
        private static readonly ILinkCache<ISkyrimMod, ISkyrimModGetter> _linkCache;

        private const string _modName = "SAVE_ActorsChanges.esp";
        private static readonly string _skyrimDataPath;
        private static readonly string _outputModPath;
        private static readonly SkyrimMod _outputMod;

        static GameContext()
        {
            if (!GameLocations.TryGetDataFolder(GameRelease.SkyrimSE, out var pathSkyrimData))
                throw new Exception("Skyrim SE is not installed.");

            _skyrimDataPath = pathSkyrimData;
            _outputMod = GetOrCreateMod(_modName);
            _outputModPath = System.IO.Path.Combine(_skyrimDataPath, _modName);

            _environment = GameEnvironment.Typical.Builder<ISkyrimMod, ISkyrimModGetter>(GameRelease.SkyrimSE)
                .TransformLoadOrderListings(mods => mods.Where(mod => mod.Enabled))
                .WithOutputMod(_outputMod)
                .Build();

            _linkCache = _environment.LinkCache;
        }

        public static IEnumerable<INpcGetter> LoadNPCs()
        {
            foreach (var npc in _environment.LoadOrder.PriorityOrder.Npc().WinningOverrides())
                yield return npc;
        }

        public static bool TryResolve<TExpected>(IFormLinkGetter<TExpected> link, [MaybeNullWhen(false)] out TExpected record)
            where TExpected : class, ISkyrimMajorRecordGetter
        {
            return _linkCache.TryResolve(link, out record);
        }

        public static T GetOriginalOrOverride<T>(T originalRecord)
            where T : class, ISkyrimMajorRecordGetter
        {
            return _outputMod.EnumerateMajorRecords<T>()
                    .FirstOrDefault(r => r.FormKey == originalRecord.FormKey)
                ?? originalRecord;
        }

        public static TEditable GetAsOverride<TReadOnly, TEditable>(TReadOnly record)
            where TReadOnly : class, ISkyrimMajorRecordGetter
            where TEditable : class, ISkyrimMajorRecord, TReadOnly
        {
            return _outputMod.GetTopLevelGroup<TEditable>()
                .GetOrAddAsOverride(record);
        }

        public static void SaveChanges()
        {
            _outputMod.WriteToBinary(_outputModPath);
        }

        private static SkyrimMod GetOrCreateMod(string modName)
        {
            var path = System.IO.Path.Combine(_skyrimDataPath, modName);

            if (System.IO.File.Exists(path))
            {
                return SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);
            }

            return new SkyrimMod(modName, SkyrimRelease.SkyrimSE, 1.7f);
        }
    }
}