using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ActorBaseNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            if (npc.PlayerSkills == null)
                yield break;

            var baseActorValues = new (string name, string source, Func<INpcGetter, float> getter, Action<INpc, float> setter)[]
            {
                ("Health", "Offset", r => r.Configuration.HealthOffset, (r, v) => r.Configuration.HealthOffset = (short)v),
                ("Magicka", "Offset", r => r.Configuration.MagickaOffset, (r, v) => r.Configuration.MagickaOffset = (short)v),
                ("Stamina", "Offset", r => r.Configuration.StaminaOffset, (r, v) => r.Configuration.StaminaOffset = (short)v),
                ("SpeedMult", "Base", r => r.Configuration.SpeedMultiplier, (r, v) => r.Configuration.SpeedMultiplier = (short)v),
            };

            foreach (var (name, source, getter, setter) in baseActorValues)
                yield return new(name, new ActorBaseNode(source, npc, getter, setter));

            foreach (var skill in npc.PlayerSkills.SkillValues.Keys)
            {
                yield return new(skill.ToString(), new ActorBaseNode(
                    "SkillValues", npc,
                    r => r.PlayerSkills.SkillValues[skill],
                    (r, v) => r.PlayerSkills.SkillValues[skill] = (byte)v)
                );
            }

            foreach (var skill in npc.PlayerSkills.SkillOffsets.Keys)
            {
                yield return new(skill.ToString(), new ActorBaseNode(
                    "SkillOffsets", npc,
                    r => r.PlayerSkills.SkillOffsets[skill],
                    (r, v) => r.PlayerSkills.SkillOffsets[skill] = (byte)v)
                );
            }
        }
    }
}