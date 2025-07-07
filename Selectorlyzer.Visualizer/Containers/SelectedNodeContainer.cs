using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public interface ISelectedNodeContainer
{
    SyntaxNode? Selector { get; set; }

    string? Expression { get; }

    IEnumerable<PropertyInfo>? SelectorProperties { get; }

    event Func<Task>? OnChange;
}

public class SelectedNodeContainer : ISelectedNodeContainer
{
    private SyntaxNode? selector;

    private string? expression { get; set; }

    private IEnumerable<PropertyInfo>? selectedNodeProperties { get; set; }

    public SyntaxNode? Selector
    {
        get => selector;
        set
        {
            selector = value;
            GetProperties();
            GetExpression();
            NotifyStateChanged();
        }
    }

    public string? Expression
    {
        get => expression;
    }

    public IEnumerable<PropertyInfo>? SelectorProperties
    {
        get => selectedNodeProperties;
    }

    public event Func<Task>? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    private List<string> propertiesToExcldue =
    [
        "Language",
            "FullSpan",
            "Span",
            "SpanStart",
            "ParentTrivia",
            "OpenParenToken",
            "CloseParenToken",
            "OpenBraceToken",
            "CloseBraceToken",
            "SemicolonToken",
            "Parent",
            "RawKind",
        ];

    private void GetProperties()
    {

        selectedNodeProperties = Selector
            ?.GetType()
            .GetProperties()
            .Where(x => !propertiesToExcldue.Contains(x.Name)) ?? [];
    }

    private void GetExpression()
    {
        var results = new List<string>();
        var currentNode = selector;
        while (currentNode?.Parent != null)
        {
            if (currentNode.Parent is IdentifierNameSyntax)
            {
                break;
            }

            results.Add(GetNodeExpression(currentNode));
            currentNode = currentNode.Parent;
        }

        results.Reverse();

        expression = string.Join(" > ", results);
    }

    private static string GetNodeExpression(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
        {
            return $":class[Name='{classDeclarationSyntax.Identifier.Text}']";
        }
        else if (syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax)
        {
            return $":method[Name='{methodDeclarationSyntax.Identifier.Text}']";
        }
        else if (syntaxNode is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
        {
            return $":namespace[Name='{namespaceDeclarationSyntax.Name}']";
        }
        else if (syntaxNode is InvocationExpressionSyntax invocationExpressionSyntax)
        {
            return $"InvocationExpression[Expression='{invocationExpressionSyntax.Expression}']";
        }

        var name = GetFriendlyName(syntaxNode);
        if (name != null)
        {
            return $"{syntaxNode.Kind()}[Name='{name}']";
        }

        return $"{syntaxNode.Kind()}";
    }

    private static string? GetFriendlyName(SyntaxNode syntaxNode)
    {
        return syntaxNode switch
        {
            MethodDeclarationSyntax methodDeclSyntax => methodDeclSyntax.Identifier.ToString(),
            PropertyDeclarationSyntax propertyDeclSyntax =>
                propertyDeclSyntax.Identifier.ToString(),
            TypeDeclarationSyntax typeDeclSyntax => typeDeclSyntax.Identifier.ToString(),
            ParameterSyntax paramSyntax => paramSyntax.Identifier.ToString(),
            NameSyntax nameSyntax => nameSyntax.ToString(),
            _ => default,
        };
    }
}