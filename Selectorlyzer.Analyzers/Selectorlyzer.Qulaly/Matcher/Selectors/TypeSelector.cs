using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
}
