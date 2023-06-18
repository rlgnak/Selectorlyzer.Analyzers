using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class LambdaPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is LambdaExpressionSyntax;
        }

        public override string ToSelectorString()
        {
            return ":lambda";
        }
    }
}
