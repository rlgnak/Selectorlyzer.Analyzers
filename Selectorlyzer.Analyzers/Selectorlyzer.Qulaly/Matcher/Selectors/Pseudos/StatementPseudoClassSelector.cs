using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class StatementPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is StatementSyntax;
        }

        public override string ToSelectorString()
        {
            return ":statement";
        }
    }
}
