using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Selectorlyzer.Qulaly.Matcher.Selectors.Properties
{
    public abstract class PropertySelector : Selector
    {
        public string PropertyName { get; }

        public PropertySelector(string propertyName)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        protected string? GetFriendlyName(in SelectorMatcherContext ctx)
        {
            return ctx.Node switch
            {
                AttributeSyntax attrSyntax => attrSyntax.Name.ToString(),
                NamespaceDeclarationSyntax attrSyntax => attrSyntax.Name.ToString(),
                MethodDeclarationSyntax methodDeclSyntax => methodDeclSyntax.Identifier.ToString(),
                PropertyDeclarationSyntax propertyDeclSyntax => propertyDeclSyntax.Identifier.ToString(),
                TypeDeclarationSyntax typeDeclSyntax => typeDeclSyntax.Identifier.ToString(),
                ParameterSyntax paramSyntax => paramSyntax.Identifier.ToString(),
                NameSyntax nameSyntax => nameSyntax.ToString(),
                _ => default,
            };
        }

        protected string? GetPropertyValue(in SelectorMatcherContext ctx)
        {
            if (PropertyName == "Name")
            {
                return GetFriendlyName(ctx);
            }

            var prop = ctx.Node.GetType().GetProperty(PropertyName);
            return prop?.GetValue(ctx.Node)?.ToString();
        }
    }
}
