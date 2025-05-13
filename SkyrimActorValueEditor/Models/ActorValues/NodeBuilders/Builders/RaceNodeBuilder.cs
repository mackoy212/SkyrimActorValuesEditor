using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Interfaces;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Base;
using SkyrimActorValueEditor.Models.ActorValues.Nodes.Records;

namespace SkyrimActorValueEditor.Models.ActorValues.NodeBuilders.Builders
{
    public class RaceNodeBuilder : ITreeNodeBuilder<INpcGetter>
    {
        private readonly SpellNodeBuilder _spellNodeBuilder = new();

        public IEnumerable<KeyValuePair<string, TreeNode>> TryBuild(INpcGetter npc)
        {
            if (!GameContext.TryResolve(npc.Race, out var race))
                yield break;

            foreach (var spell in GetActorEffects(race))
            {
                foreach (var (actorValue, spellNode) in _spellNodeBuilder.TryBuild(spell))
                {
                    var raceNode = new RaceNode(race);
                    raceNode.AddNode(spellNode);
                    yield return new(actorValue, raceNode);
                }
            }

            foreach (var actorValue in race.Starting.Keys)
            {
                yield return new(
                    actorValue.ToString(),
                    new RaceNode(race, r => r.Starting[actorValue])
                );
            }

            var skillBoosts = new[]
            {
                race.SkillBoost0,
                race.SkillBoost1,
                race.SkillBoost2,
                race.SkillBoost3,
                race.SkillBoost4,
                race.SkillBoost5,
                race.SkillBoost6
            };

            foreach (var skillBoost in skillBoosts)
            {
                if (skillBoost.Skill == ActorValue.None)
                    continue;

                yield return new(
                    skillBoost.Skill.ToString(),
                    new RaceNode(race, r => skillBoost.Boost)
                );
            }
        }

        private static IEnumerable<ISpellGetter> GetActorEffects(IRaceGetter race)
        {
            if (race.ActorEffect == null)
                yield break;

            foreach (var spellLink in race.ActorEffect)
            {
                if (GameContext.TryResolve(spellLink, out var spellRecord)
                    && spellRecord is ISpellGetter spell)
                {
                    yield return spell;
                }
            }
        }
    }
}
