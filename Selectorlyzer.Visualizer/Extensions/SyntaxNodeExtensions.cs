using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class SyntaxNodeExtensions
{
    public static SyntaxNode? FindNodeByLineColumn(
            this SyntaxTree syntaxTree,
            int targetLine,
            int targetColumn
        )
    {
        try
        {
            var root = syntaxTree.GetRoot();
            var sourceText = syntaxTree.GetText();

            var textLine = sourceText.Lines[targetLine - 1];
            int position = textLine.Start + targetColumn;

            var token = root.FindToken(position);

            var matchingNode = root.FindNode(token.Span, getInnermostNodeForTie: true);

            if (matchingNode is IdentifierNameSyntax)
            {
                matchingNode = matchingNode.Parent;
            }

            return matchingNode;
        }
        catch
        {
            return null;
        }
    }
}