namespace Selectorlyzer.Analyzers;

public class SelectorlyzerRule
{
    public string Selector { get; set; } = null!;

    public string? Rule { get; set; }

    public string Message { get; set; } = null!;

    public string? Severity { get; set; }

    public bool IncludeGeneratedCode { get; set; }
}