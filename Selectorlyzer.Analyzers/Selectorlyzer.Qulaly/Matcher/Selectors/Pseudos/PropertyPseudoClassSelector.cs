using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    /// <summary>
    /// Custom
    /// </summary>
    public class PropertyPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node.IsKind(SyntaxKind.PropertyDeclaration);
        }

        public override string ToSelectorString()
        {
            return ":property";
        }
    }
}
