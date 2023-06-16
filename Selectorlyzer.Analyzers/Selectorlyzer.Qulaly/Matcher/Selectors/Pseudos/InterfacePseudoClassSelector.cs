using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class InterfacePseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is InterfaceDeclarationSyntax;
        }

        public override string ToSelectorString()
        {
            return ":interface";
        }
    }
}
