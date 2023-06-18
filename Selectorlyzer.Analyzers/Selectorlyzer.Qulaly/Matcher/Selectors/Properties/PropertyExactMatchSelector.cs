namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertyExactMatchSelector : PropertySelector
    {
        public string Value { get; }

        public PropertyExactMatchSelector(string propertyName, string value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                return GetPropertyValue(ctx)?.Equals(Value) ?? false;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}='{Value}']";
        }
    }
}
