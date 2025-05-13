using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Skyrim;
using SkyrimActorValueEditor.Core.Services;
using SkyrimActorValueEditor.ViewModels.Utils;

namespace SkyrimActorValueEditor.Core.Extensions
{
    public static class RecordExtensions
    {
        public static string? GetName<T>(this T record)
            where T : ITranslatedNamedRequiredGetter
        {
            return EncodingUtils.Convert1252ToUTF8(record.Name.String);
        }
        public static string ToDisplayString(this IConditionGetter condition)
        {
            string conditionObject = GetConditionObject(condition);
            string conditionOperator = GetConditionCompareOperator(condition);
            float value = GetConditionValue(condition);
            string flag = GetConditionFlag(condition);

            return $"{condition.Data.RunOnType}.{condition.Data.Function} {conditionObject} {conditionOperator} {value} {flag}";
        }

        #region Conditions

        private static string GetConditionObject(IConditionGetter condition)
        {
            string? conditionObject = condition.Data switch
            {
                IHasPerkConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Perk.Link, out var perk) ? perk.EditorID : null,

                IHasMagicEffectConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.MagicEffect.Link, out var effect) ? effect.EditorID : null,

                IWornHasKeywordConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Keyword.Link, out var keyword) ? keyword.EditorID : null,

                IHasKeywordConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Keyword.Link, out var keyword) ? keyword.EditorID : null,

                IHasSpellConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Spell.Link, out var spell) ? spell.EditorID : null,

                IGetGlobalValueConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Global.Link, out var global) ? global.EditorID : null,

                IGetEquippedItemTypeConditionDataGetter conditionData =>
                    conditionData.ItemSource.ToString(),

                IGetPCMiscStatConditionDataGetter conditionData =>
                    conditionData.MiscStat.ToString(),

                IGetActorValuePercentConditionDataGetter conditionData =>
                    conditionData.ActorValue.ToString(),

                _ => null
            };

            return conditionObject ?? "NONE";
        }

        private static string GetConditionCompareOperator(IConditionGetter condition)
        {
            return condition.CompareOperator switch
            {
                CompareOperator.EqualTo => "==",
                CompareOperator.NotEqualTo => "!=",
                CompareOperator.GreaterThan => ">",
                CompareOperator.GreaterThanOrEqualTo => ">=",
                CompareOperator.LessThan => "<",
                CompareOperator.LessThanOrEqualTo => "<=",
                _ => string.Empty,
            };
        }

        private static float GetConditionValue(IConditionGetter condition)
        {
            switch (condition)
            {
                case IConditionFloatGetter conditionFloat:
                    return conditionFloat.ComparisonValue;
                case IConditionGlobalGetter conditionGlobal:
                    switch (conditionGlobal)
                    {
                        case IGlobalShortGetter globalShort:
                            return globalShort.Data!.Value;
                        case IGlobalFloatGetter globalFloat:
                            return globalFloat.Data!.Value;
                        case IGlobalIntGetter globalInt:
                            return globalInt.Data!.Value;
                        default: return 0.0f;
                    }
                default: return 0.0f;
            }
        }

        private static string GetConditionFlag(IConditionGetter condition)
        {
            if (condition.Flags.HasFlag(Condition.Flag.OR))
            {
                return "OR";
            }

            return "AND";
        }

        #endregion
    }
}