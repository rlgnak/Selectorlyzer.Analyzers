namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertyPrefixMatchSelector : PropertySelector
    {
        public string Value { get; }

        public PropertyPrefixMatchSelector(string propertyName, string value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                return GetPropertyValue(ctx)?.StartsWith(Value) ?? false;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}^='{Value}']";
        }
    }
}
