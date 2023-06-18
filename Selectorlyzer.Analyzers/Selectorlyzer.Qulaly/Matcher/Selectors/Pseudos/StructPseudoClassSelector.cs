using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class StructPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is StructDeclarationSyntax;
        }

        public override string ToSelectorString()
        {
            return ":struct";
        }
    }
}
