using Selectorlyzer.Qulaly;

public interface IQualySelectorContainer
{
    QulalySelector? Selector { get; set; }

    event Func<Task>? OnChange;
}

public class QualySelectorContainer : IQualySelectorContainer
{
    private QulalySelector? selector;

    public QulalySelector? Selector
    {
        get => selector;
        set
        {
            selector = value;
            NotifyStateChanged();
        }
    }

    public event Func<Task>? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}