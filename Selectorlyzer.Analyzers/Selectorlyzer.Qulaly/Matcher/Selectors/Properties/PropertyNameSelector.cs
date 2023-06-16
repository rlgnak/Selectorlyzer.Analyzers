namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertyNameSelector : PropertySelector
    {
        public PropertyNameSelector(string propertyName)
            : base(propertyName)
        {
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                var prop = ctx.Node.GetType().GetProperty(PropertyName);
                return prop != null;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}]";
        }
    }
}
