using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertyItemContainsMatchSelector : PropertySelector
    {
        public string Value { get; }

        public PropertyItemContainsMatchSelector(string propertyName, string value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            if (Value == string.Empty) return (in SelectorMatcherContext ctx) => false;

            return (in SelectorMatcherContext ctx) =>
            {
                if (PropertyName == "Modifiers" && ctx.Node is MemberDeclarationSyntax memberDeclarationSyntax)
                {
                    return memberDeclarationSyntax.Modifiers.Any(x => x.ValueText == Value);
                }

                return GetPropertyValue(ctx)?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(Value) ?? false;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}~='{Value}']";
        }
    }
}
