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
                var friendlyName = GetFriendlyName(ctx);
                if (friendlyName != null)
                {
                    return friendlyName;
                }
            }

            var prop = ctx.Node.GetType().GetProperty(PropertyName);
            return prop?.GetValue(ctx.Node)?.ToString();
        }
    }
}
