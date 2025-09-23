using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface ICodeContainer
{
    string? Code { get; set; }

    CompilationUnitSyntax? CompilationUnit { get; }

    event Func<Task>? OnChange;
}

public class CodeContainer : ICodeContainer
{
    private string? code;

    private CompilationUnitSyntax? compilationUnit;

    public CodeContainer(ILogger<CodeContainer> logger)
    {
        Code = """
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
""";
        Logger = logger;
    }

    public string? Code
    {
        get => code;
        set
        {
            code = value;
            CreateCompilationUnitSyntax();
            NotifyStateChanged();
        }
    }

    public CompilationUnitSyntax? CompilationUnit
    {
        get => compilationUnit;
    }
    public ILogger<CodeContainer> Logger { get; }

    public event Func<Task>? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    private void CreateCompilationUnitSyntax()
    {
        try
        {
            if (code != null)
            {
                compilationUnit = SyntaxFactory.ParseCompilationUnit(code);
            }
            else
            {
                compilationUnit = null;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error during CreateCompilationUnitSyntax");
            compilationUnit = null;
        }
    }
}