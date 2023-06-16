namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public class PropertyEqualMatchSelector : PropertySelector
    {
        public int Value { get; }

        public PropertyEqualMatchSelector(string propertyName, int value)
            : base(propertyName)
        {
            Value = value;
        }

        public override SelectorMatcher GetMatcher()
        {
            return (in SelectorMatcherContext ctx) =>
            {
                return int.TryParse(GetPropertyValue(ctx), out var value) && value == Value;
            };
        }

        public override string ToSelectorString()
        {
            return $"[{PropertyName}={Value}]";
        }
    }
}
