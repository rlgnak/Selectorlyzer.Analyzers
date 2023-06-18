namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertySubstringMatchSelector : PropertySelector
    {
        public string Value { get; }

        public PropertySubstringMatchSelector(string propertyName, string value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                return GetPropertyValue(ctx)?.Contains(Value) ?? false;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}*='{Value}']";
        }
    }
}
