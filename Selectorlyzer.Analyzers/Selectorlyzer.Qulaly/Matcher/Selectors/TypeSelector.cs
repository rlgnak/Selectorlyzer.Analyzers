using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Selectorlyzer.Qulaly.Matcher;

namespace Selectorlyzer.Qulaly.Matcher.Selectors
{
    public class TypeSelector : Selector
    {
        public SyntaxKind Kind { get; }

        public TypeSelector(SyntaxKind kind)
        {
            Kind = kind;
        }

        // Type Selector
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node.IsKind(Kind);
        }

        public override string ToSelectorString()
        {
            return Kind.ToString();
        }
    }

    public class UniversalTypeSelector : Selector
    {

        public UniversalTypeSelector()
        {
        }

        // Type Selector
        public override SelectorMatcher GetMatcher()
        {
            // Wildcard selector
            return (in SelectorMatcherContext ctx) => true;
        }

        public override string ToSelectorString()
        {
            return "*";
        }
    }
}
