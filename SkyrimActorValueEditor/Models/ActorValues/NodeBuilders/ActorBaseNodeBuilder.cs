using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.RecordNodes.Base;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders
{
    public class ActorBaseNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            if (npc.PlayerSkills == null)
                yield break;

            var baseActorValues = new (string actorValue, string source, Func<INpcGetter, float> getter, Action<INpc, float> setter)[]
            {
                ("Health", "Offset", r => r.Configuration.HealthOffset, (r, v) => r.Configuration.HealthOffset = (short)v),
                ("Magicka", "Offset", r => r.Configuration.MagickaOffset, (r, v) => r.Configuration.MagickaOffset = (short)v),
                ("Stamina", "Offset", r => r.Configuration.StaminaOffset, (r, v) => r.Configuration.StaminaOffset = (short)v),
                ("SpeedMult", "Base", r => r.Configuration.SpeedMultiplier, (r, v) => r.Configuration.SpeedMultiplier = (short)v),
            };

            foreach (var (actorValue, source, getter, setter) in baseActorValues)
            {
                var accessor = new MajorRecordValueAccessor<INpcGetter, Npc>(getter, setter);
                yield return new(actorValue, new ActorBaseNode(source, npc, accessor));
            }

            foreach (var skill in npc.PlayerSkills.SkillValues.Keys)
            {
                var accessor = new MajorRecordValueAccessor<INpcGetter, Npc>(
                    r => r.PlayerSkills.SkillValues[skill],
                    (r, v) => r.PlayerSkills.SkillValues[skill] = (byte)v
                );
                yield return new(skill.ToString(), new ActorBaseNode("SkillValues", npc, accessor));
            }

            foreach (var skill in npc.PlayerSkills.SkillOffsets.Keys)
            {
                var accessor = new MajorRecordValueAccessor<INpcGetter, Npc>(
                    r => r.PlayerSkills.SkillOffsets[skill],
                    (r, v) => r.PlayerSkills.SkillOffsets[skill] = (byte)v
                );
                yield return new(skill.ToString(), new ActorBaseNode("SkillOffsets", npc, accessor));
            }
        }
    }
}