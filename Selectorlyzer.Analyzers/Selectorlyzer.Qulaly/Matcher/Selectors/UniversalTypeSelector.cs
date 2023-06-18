namespace Selectorlyzer.Qulaly.Matcher.Selectors
{
    public class UniversalTypeSelector : Selector
    {

        public UniversalTypeSelector()
        {
        }

        // Type Selector
        public override SelectorMatcher GetMatcher()
        {
            // Wildcard selector
            return (in SelectorMatcherContext ctx) => true;
        }

        public override string ToSelectorString()
        {
            return "*";
        }
    }
}
