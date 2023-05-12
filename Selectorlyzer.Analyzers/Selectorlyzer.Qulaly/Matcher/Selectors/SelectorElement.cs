namespace Selectorlyzer.Qulaly.Matcher.Selectors
{
    public abstract class SelectorElement
    {
        public abstract string ToSelectorString();

        public override string ToString()
        {
            return $"{GetType().Name}: {ToSelectorString()}";
        }
    }
}