using Selectorlyzer.Qulaly.Helpers;
using System;
using System.Linq;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    public class NthChildPseudoClassSelector : PseudoClassSelector
    {
        private readonly string _expression;

        public NthChildPseudoClassSelector(string expression)
        {
            _expression = expression;
        }

        public override SelectorMatcher GetMatcher()
        {
            var (offset, step) = NthHelper.GetOffsetAndStep(_expression);

            return (in SelectorMatcherContext ctx) =>
            {
                var parent = ctx.Node.Parent;

                if (parent == null)
                {
                    return false;
                }

                var index = 0;
                foreach (var node in parent.ChildNodes())
                {
                    if (node == ctx.Node)
                    {
                        return NthHelper.IndexMatchesOffsetAndStep(index, offset, step);
                    }
                    index++;
                }

                return false;
            };
        }

        public override string ToSelectorString()
        {
            return $":nth-child({_expression})";
        }
    }
}
