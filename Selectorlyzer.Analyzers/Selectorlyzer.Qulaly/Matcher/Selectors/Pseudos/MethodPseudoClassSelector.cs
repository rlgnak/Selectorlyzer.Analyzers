using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class MethodPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node.IsKind(SyntaxKind.MethodDeclaration);
        }

        public override string ToSelectorString()
        {
            return ":method";
        }
    }
}
