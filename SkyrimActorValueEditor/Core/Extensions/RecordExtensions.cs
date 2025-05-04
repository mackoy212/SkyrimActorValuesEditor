using Mutagen.Bethesda.Plugins.Aspects;
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

        #region Conditions

        /*public static string GetConditionData(IConditionGetter condition)
        {
            string conditionObject = GetConditionObject(condition);
            string conditionOperator = GetConditionCompareOperator(condition);
            float value = GetConditionValue(condition);
            string flag = GetConditionFlag(condition);

            return $"{condition.Data.RunOnType}.{condition.Data.Function} {conditionObject} {conditionOperator} {value} {flag}";
        }

        private static string GetConditionObject(IConditionGetter condition)
        {
*//*            ISkyrimMajorRecordGetter? conditionObject = condition.Data switch
            {
                IHasPerkConditionDataGetter conditionData => conditionData.Perk.Link.TryResolve(GameContext.LinkCache),
                IHasMagicEffectConditionDataGetter conditionData => conditionData.MagicEffect.Link.TryResolve(GameContext.LinkCache),
                IWornHasKeywordConditionDataGetter conditionData => conditionData.Keyword.Link.TryResolve(GameContext.LinkCache),
                _ => null
            };

            return conditionObject?.EditorID?.ToString() ?? "NONE";*//*
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
        }*/

        #endregion
    }
}