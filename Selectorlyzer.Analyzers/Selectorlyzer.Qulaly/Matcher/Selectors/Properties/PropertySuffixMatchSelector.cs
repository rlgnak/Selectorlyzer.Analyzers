namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertySuffixMatchSelector : PropertySelector
    {
        public string Value { get; }

        public PropertySuffixMatchSelector(string propertyName, string value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                return GetPropertyValue(ctx)?.EndsWith(Value) ?? false;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}$='{Value}']";
        }
    }
}
