using Mutagen.Bethesda;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.Npcs;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Core.Services.GameData
{
    public static class GameReader
    {
        public static ILinkCache<ISkyrimMod, ISkyrimModGetter> LinkCache => _linkCache;

        private static readonly IGameEnvironment<ISkyrimMod, ISkyrimModGetter> _environment;
        private static readonly ILinkCache<ISkyrimMod, ISkyrimModGetter> _linkCache;

        private static readonly SkyrimMod _outputMod = GamePlugin.OutputMod;

        static GameReader()
        {
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
    }
}