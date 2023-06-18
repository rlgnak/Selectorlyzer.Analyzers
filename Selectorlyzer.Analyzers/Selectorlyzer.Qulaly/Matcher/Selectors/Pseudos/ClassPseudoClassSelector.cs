using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class ClassPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is ClassDeclarationSyntax;
        }

        public override string ToSelectorString()
        {
            return ":class";
        }
    }
}
