using System.Linq;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class LastChildPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node == ctx.Node.Parent!.ChildNodes().Last();
        }

        public override string ToSelectorString()
        {
            return ":last-child";
        }
    }
}
