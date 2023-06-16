using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    /// <summary>
    /// Custom
    /// </summary>
    public class ImplementsPseudoClassSelector : PseudoClassSelector
    {
        private readonly Selector[] _relativeSelectors;

        public ImplementsPseudoClassSelector(params Selector[] relativeSelectors)
        {
            _relativeSelectors = relativeSelectors ?? throw new ArgumentNullException(nameof(relativeSelectors));
        }

        public override SelectorMatcher GetMatcher()
        {
            SelectorMatcher matcher = (in SelectorMatcherContext _) => false;
            foreach (var selector in _relativeSelectors)
            {
                matcher = SelectorCompilerHelper.ComposeOr(matcher, selector.GetMatcher());
            }

            var query = new QulalySelector(matcher, this);

            return (in SelectorMatcherContext ctx) =>
            {
                return ctx.Node.QuerySelector("BaseList SimpleBaseType")?.QuerySelector(query) != null;
            };
        }

        public override string ToSelectorString()
        {
            return $":implements({string.Join(",", _relativeSelectors.Select(x => x.ToSelectorString()))})";
        }
    }
}
