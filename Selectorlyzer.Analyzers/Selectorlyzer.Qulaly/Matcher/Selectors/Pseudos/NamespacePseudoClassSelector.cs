using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class NamespacePseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node is NamespaceDeclarationSyntax || ctx.Node is FileScopedNamespaceDeclarationSyntax;
        }

        public override string ToSelectorString()
        {
            return ":namespace";
        }
    }
}
