# Selectorlyzer Analyzers for the .NET Compiler Platform

[![NuGet](https://img.shields.io/nuget/v/Selectorlyzer.Analyzers.svg)](https://www.nuget.org/packages/Selectorlyzer.Analyzers)
[![Build Status](https://github.com/rlgnak/Selectorlyzer.Analyzers/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rlgnak/Selectorlyzer.Analyzers/actions/workflows/dotnet.yml)

Selectorlyzer.Analyzers is a configurable Roslyn Analyzer that uses CSS selector-like syntax for enforcing project specific conventions. 

<img src="https://github.com/rlgnak/Selectorlyzer.Analyzers/assets/1643317/a56e8fef-1e42-47b4-acbf-7be884f91d6f" width="453" height="250">

## Using Selectorlyzer.Analyzers

The preferable way to use the analyzers is to add the NuGet package Selectorlyzer.Analyzers to the project where you want to enforce rules.

A `selectorlyzer.json` or `.selectorlyzer.json` file is used to specify rules. 

## Installation

1. Install `Selectorlyzer.Analyzers`.
1. Create and configure `selectorlyzer.json` file.

```json
{
  "rules": [
    {
      "selector": ":class:has([Name='InvalidClassName'])",
      "message": "Classes should not be named 'InvalidClassName'",
      "severity": "error"
    },
    {
      "selector": "InvocationExpression[Expression='Console.WriteLine']",
      "message": "Do not use Console.WriteLine",
      "severity": "error"
    }
  ]
}
```

1. Add the following to the `.csproj` file
```xml
<ItemGroup>
  <AdditionalFiles Include="selectorlyzer.json" />
</ItemGroup>
```

## Examples

The following will raise an error diagnostic if a class is named `InvalidClassName`.
```json
{
    "selector": ":class:has([Name='InvalidClassName'])",
    "message": "Classes should not be named 'InvalidClassName'",
    "severity": "error"
}
```

The following will raise a warning diagnostic if a class starts with `InvalidPrefix`.
```json
{
    "selector": ":class:has([Name^='InvalidPrefix'])",
    "message": "Classes should not start with `InvalidPrefix`",
    "severity": "warning"
}
```

The following will raise a warning diagnostic if a class ends with `InvalidSuffix`.
```json
{
    "selector": ":class:has([Name^='InvalidSuffix'])",
    "message": "Classes should not be end with `InvalidSuffix`",
    "severity": "warning"
}
```

The following will raise an error diagnostic if the function `Console.WriteLine` is invoked.
```json
{
    "selector": "InvocationExpression[Expression='Console.WriteLine']",
    "message": "Do not use Console.WriteLine",
    "severity": "error"
}
```

The following will raise an error diagnostic if a method is named `DoNotUseMe`.
```json
{
    "selector": ":method:has([Name='DoNotUseMe'])",
    "message": "The method 'DoNotUseMe' should not be used",
    "severity": "error"
}
```

The following will raise an info diagnostic if a class within the namespace `Repositories` does not implement an self interface.
```json
{
    "selector": ":namespace[Name$='Repositories'] :class",
    "rule": ":implements([Name='I{Name}'])",
    "message": "Classes within the 'Repositories' namespace should implement a self interface",
    "severity": "info"
}
```

The following rule will raise an error if a class ending with `Controller` contains public methods without a `Http*` attribute.
```json
{
    "selector": ":class[Name$='Controller'] :method[Modifiers~='public']",
    "rule": "Attribute[Name^='Http']",
    "message": "Public methods on controllers should be decorated with a `Http*` attribute",
    "severity": "error"
}
```

## Selectors 

Selectorlyzer uses a custom version of [Qulaly](https://github.com/mayuki/Qulaly) a query langage for Roslyn Inspired by [esquery](https://github.com/estools/esquery). These selectors are used to identify speicifc sytax nodes.

### Supported Selectors

Qulaly supports a subset of [CSS selector level 4](https://www.w3.org/TR/selectors-4/). The selector engine also supports Qulaly-specific extensions to the selector.

- SyntaxNode Type: `MethodDeclaration`, `ClassDeclaration` ... 
    - See also [SyntaxKind enum](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind?view=roslyn-dotnet)
- SyntaxNode Univarsal: `*`
- SyntaxNode pseudo-classes (for short-hand)
    - `:method`
    - `:class`
    - `:interface`
    - `:lambda`
- Combinators
    - [Descendant](https://www.w3.org/TR/selectors-4/#descendant-combinators): `node descendant`
    - [Child](https://www.w3.org/TR/selectors-4/#child-combinators): `node > child`
    - [Next-sibling](https://www.w3.org/TR/selectors-4/#adjacent-sibling-combinators): `node + next`
    - [Subsequent-sibling](https://www.w3.org/TR/selectors-4/#general-sibling-combinators): `node ~ sibling`
- Pseudo-class
    - [Negation](https://www.w3.org/TR/selectors-4/#negation): `:not(...)`
    - [Matches-any](https://www.w3.org/TR/selectors-4/#matches): `:is(...)`
    - [Relational](https://www.w3.org/TR/selectors-4/#relational): `:has(...)`
    - [`:first-child`](https://www.w3.org/TR/selectors-4/#the-first-child-pseudo)
    - [`:last-child`](https://www.w3.org/TR/selectors-4/#the-last-child-pseudo)
- Attributes (Properties)
    - `[PropName]` (existance)
    - `[PropName = 'Exact']`
    - `[PropName ^= 'StartsWith']`
    - `[PropName $= 'EndsWith']`
    - `[PropName *= 'Contains']`
    - `[PropName ~= 'Item']` (ex. `[Modifiers ~= 'async']`)
- Qulaly Extensions
    - `:implements(...)`: Combinator for checking if a class or interface implements a matching selector
    - `[Name = 'MethodName']`: Name special property
        - `Name` is a special property for convenience that can be used in `MethodDeclaration`, `ClassDeclaration` ... etc
    - `[TypeParameters.Count > 0]`: Conditions
        - `Parameters.Count`
        - `TypeParameters.Count`

## License
MIT License
```
Selectorlyzer.Analyzers Copyright © 2023-present Richard Graves <rlgnak+selectorlyzer@gmail.com>
Qulaly Copyright © 2020-present Mayuki Sawatari <mayuki@misuzilla.org>
```