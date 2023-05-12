using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Selectorlyzer.Qulaly.Matcher;

namespace Selectorlyzer.Qulaly
{
    public static class QulalyExtensions
    {
        public static IEnumerable<SyntaxNode> QuerySelectorAll(this SyntaxTree syntaxTree, string selector, Compilation? compilation = default)
        {
            return syntaxTree.GetRoot().QuerySelectorAll(selector, compilation);
        }

        public static IEnumerable<SyntaxNode> QuerySelectorAll(this SyntaxNode node, string selector, Compilation? compilation = default)
        {
            return node.QuerySelectorAll(QulalySelector.Parse(selector), compilation);
        }

        public static IEnumerable<SyntaxNode> QuerySelectorAll(this SyntaxTree syntaxTree, QulalySelector selector, Compilation? compilation = default)
        {
            return syntaxTree.GetRoot().QuerySelectorAll(selector, compilation);
        }

        public static IEnumerable<SyntaxNode> QuerySelectorAll(this SyntaxNode node, QulalySelector selector, Compilation? compilation = default)
        {
            return EnumerableMatcher.GetEnumerable(node, selector, compilation?.GetSemanticModel(node.SyntaxTree));
        }

        public static SyntaxNode? QuerySelector(this SyntaxTree syntaxTree, string selector, Compilation? compilation = default)
        {
            return syntaxTree.GetRoot().QuerySelector(selector, compilation);
        }

        public static SyntaxNode? QuerySelector(this SyntaxNode node, string selector, Compilation? compilation = default)
        {
            return node.QuerySelector(QulalySelector.Parse(selector), compilation);
        }

        public static SyntaxNode? QuerySelector(this SyntaxTree syntaxTree, QulalySelector selector, Compilation? compilation = default)
        {
            return syntaxTree.GetRoot().QuerySelector(selector, compilation);
        }

        public static SyntaxNode? QuerySelector(this SyntaxNode node, QulalySelector selector, Compilation? compilation = default)
        {
            return EnumerableMatcher.GetEnumerable(node, selector, compilation?.GetSemanticModel(node.SyntaxTree)).FirstOrDefault();
        }
    }
}
