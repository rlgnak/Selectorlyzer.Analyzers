using System.Linq;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class FirstChildPseudoClassSelector : PseudoClassSelector
    {
        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) => ctx.Node == ctx.Node.Parent!.ChildNodes().First();
        }

        public override string ToSelectorString()
        {
            return ":first-child";
        }
    }
}
