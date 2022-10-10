using MediatR;
using System;
using System.Linq.Expressions;

namespace AlgoriaCore.Application.Tests.ASanitization
{
    public static class ASanitizeRulesExtensions
    {
        public static void NotEmpty<T, TType> (this ASanitizeRuleDefinition<T, TType, string> ruleDefinition)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.NotEmpty;
        }
        public static void Empty<T, TType>(this ASanitizeRuleDefinition<T, TType, string> ruleDefinition)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.Empty;
        }
        public static void NotEmpty<T, TType>(this ASanitizeRuleDefinition<T, TType, int> ruleDefinition)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.NotEmpty;
        }
        public static void NotEmpty<T, TType>(this ASanitizeRuleDefinition<T, TType, long> ruleDefinition)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.NotEmpty;
        }
        public static void MaxLength<T, TType>(this ASanitizeRuleDefinition<T, TType, string> ruleDefinition, int length)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.MaxLength;
            ruleDefinition.Data = length;
        }
        public static void MinLength<T, TType>(this ASanitizeRuleDefinition<T, TType, string> ruleDefinition, int length)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.MinLength;
            ruleDefinition.Data = length;
        }

        public static void Equal<T, TType>(this ASanitizeRuleDefinition<T, TType, string> ruleDefinition, Expression<Func<T, string>> expression)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.Equal;

            Func<T, string> x = expression.Compile();
            var currentValue = x.Invoke(ruleDefinition.Target);

            ruleDefinition.Data = currentValue + "__"; // Esto hará que el valor de las propiedades sea diferente
        }

        public static void EmailAddress<T, TType>(this ASanitizeRuleDefinition<T, TType, string> ruleDefinition)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.EmailAddress;
        }

        public static void GreaterThan<T, TType>(this ASanitizeRuleDefinition<T, TType, byte> ruleDefinition, int valueToCompare)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.GreaterThan;
            ruleDefinition.Data = valueToCompare;
        }

        public static void GreaterThan<T, TType>(this ASanitizeRuleDefinition<T, TType, int> ruleDefinition, int valueToCompare)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.GreaterThan;
            ruleDefinition.Data = valueToCompare;
        }

        public static void GreaterThanOrEqualTo<T, TType>(this ASanitizeRuleDefinition<T, TType, int> ruleDefinition, int valueToCompare)
                    where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.GreaterThanOrEqualTo;
            ruleDefinition.Data = valueToCompare;
        }

        public static void GreaterThanOrEqualTo<T, TType>(this ASanitizeRuleDefinition<T, TType, int?> ruleDefinition, int valueToCompare)
            where T : IRequest<TType>
        {
            ruleDefinition.RuleType = RuleType.GreaterThanOrEqualTo;
            ruleDefinition.Data = valueToCompare;
        }
    }
}
