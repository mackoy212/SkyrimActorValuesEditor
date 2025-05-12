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
            ISkyrimMajorRecordGetter? conditionObject = condition.Data switch
            {
                IHasPerkConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Perk.Link, out var perk) ? perk : null,

                IHasMagicEffectConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.MagicEffect.Link, out var effect) ? effect : null,

                IWornHasKeywordConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Keyword.Link, out var keyword) ? keyword : null,

                IHasKeywordConditionDataGetter conditionData =>
                    GameContext.TryResolve(conditionData.Keyword.Link, out var keyword) ? keyword : null,

                _ => null
            };

            return conditionObject?.EditorID?.ToString() ?? "NONE";
        }

        private static string GetConditionCompareOperator(IConditionGetter condition)
        {
            switch (condition.CompareOperator)
            {
                case CompareOperator.EqualTo:
                    return "==";
                case CompareOperator.NotEqualTo:
                    return "!=";
                case CompareOperator.GreaterThan:
                    return ">";
                case CompareOperator.GreaterThanOrEqualTo:
                    return ">=";
                case CompareOperator.LessThan:
                    return "<";
                case CompareOperator.LessThanOrEqualTo:
                    return "<=";
                default:
                    return string.Empty;
            }
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