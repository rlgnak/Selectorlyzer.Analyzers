using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public abstract class PropertyCountSelector : PropertySelector
    {
        public int Value { get; }

        protected PropertyCountSelector(string propertyName, int value)
            : base(propertyName)
        {
            Value = value;
        }

        protected int? GetPropertyNumberValue(in SelectorMatcherContext ctx)
        {
            if (ctx.Node is ParameterListSyntax parameterListSyntax)
            {
                if (string.Equals(PropertyName, "Count", StringComparison.OrdinalIgnoreCase))
                {
                    return parameterListSyntax.Parameters.Count;
                }
            }
            else if (ctx.Node is TypeParameterListSyntax typeParameterListSyntax)
            {
                if (string.Equals(PropertyName, "Count", StringComparison.OrdinalIgnoreCase))
                {
                    return typeParameterListSyntax.Parameters.Count;
                }
            }
            else if (ctx.Node is AttributeListSyntax attributeListSyntax)
            {
                if (string.Equals(PropertyName, "Count", StringComparison.OrdinalIgnoreCase))
                {
                    return attributeListSyntax.Attributes.Count;
                }
            }
            else if (ctx.Node is MethodDeclarationSyntax methodDeclarationSyntax)
            {
                if (string.Equals(PropertyName, "Parameters.Count", StringComparison.OrdinalIgnoreCase))
                {
                    return methodDeclarationSyntax.ParameterList.Parameters.Count;
                }
                if (string.Equals(PropertyName, "TypeParameters.Count", StringComparison.OrdinalIgnoreCase))
                {
                    return methodDeclarationSyntax.TypeParameterList?.Parameters.Count ?? 0;
                }
            }
            else if (ctx.Node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                if (string.Equals(PropertyName, "Bases.Count", StringComparison.OrdinalIgnoreCase))
                {
                    return classDeclarationSyntax.BaseList?.Types.Count ?? 0;
                }
                if (string.Equals(PropertyName, "TypeParameters.Count", StringComparison.OrdinalIgnoreCase))
                {
                    return classDeclarationSyntax.TypeParameterList?.Parameters.Count ?? 0;
                }
                if (string.Equals(PropertyName, "AttributeLists.Count", StringComparison.OrdinalIgnoreCase))
                {
                    return classDeclarationSyntax.AttributeLists.Count;
                }
            }

            var prop = ctx.Node.GetType().GetProperty(PropertyName);
            return prop?.GetValue(ctx.Node) is int value ? value : default(int?);
        }
    }
}
