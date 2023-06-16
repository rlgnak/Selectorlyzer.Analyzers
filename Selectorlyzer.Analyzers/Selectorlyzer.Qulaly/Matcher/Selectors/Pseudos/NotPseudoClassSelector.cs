using System;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Pseudos
{
    /// <summary>
    /// 4.3. The Negation (Matches-None) Pseudo-class: :not()
    /// </summary>
    public class NotPseudoClassSelector : PseudoClassSelector
    {
        private readonly Selector _selector;

        public NotPseudoClassSelector(Selector selector)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public override SelectorMatcher GetMatcher()
        {
            var matcher = _selector.GetMatcher();

            return (in SelectorMatcherContext ctx) =>
            {
                return !matcher(ctx);
            };
        }

        public override string ToSelectorString()
        {
            return $":not({_selector.ToSelectorString()})";
        }
    }
}
