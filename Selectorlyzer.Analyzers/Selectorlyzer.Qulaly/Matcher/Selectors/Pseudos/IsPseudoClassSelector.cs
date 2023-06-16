using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    /// <summary>
    /// 4.2. The Matches-Any Pseudo-class: :is()
    /// </summary>
    public class IsPseudoClassSelector : PseudoClassSelector
    {
        private readonly Selector[] _selectors;

        public IsPseudoClassSelector(params Selector[] selectors)
        {
            _selectors = selectors ?? throw new ArgumentNullException(nameof(selectors));
        }

        public override SelectorMatcher GetMatcher()
        {
            SelectorMatcher matcher = (in SelectorMatcherContext _) => false;
            foreach (var selector in _selectors)
            {
                matcher = SelectorCompilerHelper.ComposeOr(matcher, selector.GetMatcher());
            }

            return matcher;
        }

        public override string ToSelectorString()
        {
            return $":is({string.Join(",", _selectors.Select(x => x.ToSelectorString()))})";
        }
    }
}
