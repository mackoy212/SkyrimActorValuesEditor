using Mutagen.Bethesda;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Installs;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.Npcs;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class GameContext
    {
        private static readonly IGameEnvironment<ISkyrimMod, ISkyrimModGetter> _environment;
        private static readonly ILinkCache<ISkyrimMod, ISkyrimModGetter> _linkCache;

        private const string ModName = "SAVE_ActorsChanges.esp";
        private static readonly string _pathSkyrimData;
        private static readonly string _pathOutputMod;

        private static readonly SkyrimMod _outputMod;

        static GameContext()
        {
            if (!GameLocations.TryGetDataFolder(GameRelease.SkyrimSE, out var pathSkyrimData))
                throw new Exception("Skyrim SE is not installed.");

            _pathSkyrimData = pathSkyrimData;
            _outputMod = GetOrCreateMod(ModName);
            _pathOutputMod = System.IO.Path.Combine(_pathSkyrimData, ModName);

            _environment = GameEnvironment.Typical.Builder<ISkyrimMod, ISkyrimModGetter>(GameRelease.SkyrimSE)
                .TransformLoadOrderListings(mods => mods.Where(mod => mod.Enabled))
                .WithOutputMod(_outputMod)
                .Build();

            _linkCache = _environment.LinkCache;
        }

        public static void LoadNPCs(List<NpcModel> actors)
        {
            foreach (var npc in _environment.LoadOrder.PriorityOrder.Npc().WinningOverrides())
                actors.Add(new NpcModel(npc));
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
            _outputMod.WriteToBinary(_pathOutputMod);
        }

        private static SkyrimMod GetOrCreateMod(string modName)
        {
            var path = System.IO.Path.Combine(_pathSkyrimData, modName);

            if (System.IO.File.Exists(path))
            {
                return SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);
            }

            return new SkyrimMod(modName, SkyrimRelease.SkyrimSE, 1.7f);
        }
    }
}