public interface IQueryContainer
{
    string? Selector { get; set; }

    event Func<Task>? OnChange;
}

public class QueryContainer : IQueryContainer
{
    private string? selector;

    public string? Selector
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