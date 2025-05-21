using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using SkyrimActorValueEditor.Models.Npcs;
using System.Diagnostics.CodeAnalysis;

namespace SkyrimActorValueEditor.Core.Services
{
    public static class NpcService
    {
        public static IEnumerable<NpcModel> Npcs => _npcModels.Values;

        private static readonly Dictionary<FormKey, NpcModel> _npcModels = new();

        private static readonly Random _random = new();

        static NpcService()
        {
            foreach (var npc in GameContext.LoadNPCs())
                _npcModels[npc.FormKey] = new NpcModel(npc);
        }

        public static bool TryGetNpc(FormKey formKey, [MaybeNullWhen(false)] out NpcModel npcModel)
        {
            if (_npcModels.TryGetValue(formKey, out npcModel))
                return true;

            return false;
        }

        public static INpcGetter GetNpcWithTemplates(NpcModel npcModel)
        {
            if (!npcModel.HasTemplates)
                return npcModel.NPC;

            var tempNpc = npcModel.NPC.DeepCopy();

            var randomTemplate = npcModel.Templates![_random.Next(npcModel.Templates.Count)];
            var templateNpc = GetNpcWithTemplates(randomTemplate);

            #region Traits

            if (npcModel.NPC.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Traits))
                tempNpc.Race.FormKey = templateNpc.Race.FormKey;

            #endregion

            #region Stats

            if (npcModel.NPC.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Stats)
                && npcModel.NPC.Configuration.Flags.HasFlag(NpcConfiguration.Flag.AutoCalcStats))
            {
                tempNpc.Configuration.HealthOffset = templateNpc.Configuration.HealthOffset;
                tempNpc.Configuration.MagickaOffset = templateNpc.Configuration.MagickaOffset;
                tempNpc.Configuration.StaminaOffset = templateNpc.Configuration.StaminaOffset;
                tempNpc.Configuration.SpeedMultiplier = templateNpc.Configuration.SpeedMultiplier;

                if (templateNpc.PlayerSkills != null)
                {
                    if (tempNpc.PlayerSkills == null)
                        tempNpc.PlayerSkills = new();

                    foreach (var (skill, value) in templateNpc.PlayerSkills.SkillValues)
                        tempNpc.PlayerSkills.SkillValues[skill] = value;

                    foreach (var (skill, value) in templateNpc.PlayerSkills.SkillOffsets)
                        tempNpc.PlayerSkills.SkillOffsets[skill] = value;
                }
            }

            #endregion

            #region Inventory

            if (npcModel.NPC.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Inventory))
            {
                tempNpc.DefaultOutfit.FormKey = templateNpc.DefaultOutfit.FormKey;

                if (templateNpc.Items?.Count > 0)
                {
                    tempNpc.Items = templateNpc.Items
                        .Select(x => x.DeepCopy())
                        .ToExtendedList();
                }
            }

            #endregion

            return tempNpc;
        }
    }
}