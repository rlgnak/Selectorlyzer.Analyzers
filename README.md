# Selectorlyzer Analyzers for the .NET Compiler Platform

[![NuGet](https://img.shields.io/nuget/v/Selectorlyzer.Analyzers.svg)](https://www.nuget.org/packages/Selectorlyzer.Analyzers)
[![Build Status](https://github.com/rlgnak/Selectorlyzer.Analyzers/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rlgnak/Selectorlyzer.Analyzers/actions/workflows/dotnet.yml)

Selectorlyzer.Analyzers is a highly customizable Roslyn Analyzer designed to empower developers with the ability to create project-specific analyzers using a CSS selector-like syntax.

<img src="https://github.com/rlgnak/Selectorlyzer.Analyzers/assets/1643317/a56e8fef-1e42-47b4-acbf-7be884f91d6f" width="453" height="250">

## Getting Started

The preferable way to use the analyzers is to add the NuGet package Selectorlyzer.Analyzers to the project where you want to enforce rules.

A `selectorlyzer.json` or `.selectorlyzer.json` file is used to specify rules. 

## Installation

1. Install the NuGet Package `Selectorlyzer.Analyzers`.

```console
dotnet add package Selectorlyzer.Analyzers
```

2. Create and configure `selectorlyzer.json` file.

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
    },
    {
      "selector": ":class:implements([Name='BaseRepository'])",
      "rule": ":implements([Name='I{Name}'])",
      "message": "Classes that implement 'BaseRepository' should implement an interface with the same name.",
      "severity": "error"
    }
  ]
}
```

3. Add the following to your `.csproj` files
```xml
<ItemGroup>
  <AdditionalFiles Include="selectorlyzer.json" />
</ItemGroup>
```

## Example Rules

### Naming Conventions

* `:class:has([Name='InvalidClassName'])` - Classes should not be named `InvalidClassName`
* `:class:has([Name^='InvalidPrefix'])` - Classes should not start with `InvalidPrefix`
* `:class:has([Name$='InvalidSuffix'])` - Classes should not end with `InvalidSuffix`
* `:method:has([Name='InvalidMethodName'])` - Methods should not be named `InvalidMethodName`
* `:method[Modifiers~='async']:not([Name$='Async'])` - Async method names should end with `Async`
* `:method[Modifiers~='async'][Name$='Async']` - Async method names should not end with `Async`
* `:property[Type^='bool']:not([Identifier$='Flag'])` - Boolean property names should end with `Flag`

### Custom Project Conventions

* `InvocationExpression[Expression='Console.WriteLine']` - `Console.WriteLine` should not be used.
* `InvocationExpression[Expression^='Assert.\']` - Methods starting with `Assert.` should not be used.
* `:class:implements([Name$='DataTransferObject']) ConstructorDeclaration` - Classes that implement DataTransferObject should not have constructors.
* `:class[Name$='Controller'] :method[Modifiers~='public'][ReturnType='void']` - Public methdos within classes with names ending in `Controller` should not return `void`.
* `:class[Name$='Controller'] :method[Modifiers~='public'] Attribute[Name^='Http']` - Public methods within classes with names ending in `Controller` should have an attribute that starts with `Http`
* `:class:implements([Name='BaseRepository'])` with a rule of `:implements([Name='I{Name}'])` - Classes that implement `BaseRepository` should implement an interface with the same name.
* `:method Block > * ReturnStatement` - Methods should only have on return statment and it should be the last statement in the method.


## Selectors 

Selectorlyzer uses a query langage for Roslyn Inspired by [Qulaly](https://github.com/mayuki/Qulaly) and [esquery](https://github.com/estools/esquery). These selectors are used to identify speicifc sytax nodes.

### Supported Selectors

Selectorlizer supports a subset of [CSS selector level 4](https://www.w3.org/TR/selectors-4/). The selector engine also supports Selectorlizer-specific extensions to the selector.

- SyntaxNode Type: `MethodDeclaration`, `ClassDeclaration` ... 
    - See also [SyntaxKind enum](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxkind?view=roslyn-dotnet)
- SyntaxNode Univarsal: `*`
- SyntaxNode pseudo-classes (for short-hand)
    - `:method`
    - `:class`
    - `:interface`
    - `:lambda`
    - `:property`
- Combinators
    - [Descendant](https://www.w3.org/TR/selectors-4/#descendant-combinators): `node descendant`
    - [Child](https://www.w3.org/TR/selectors-4/#child-combinators): `node > child`
    - [Next-sibling](https://www.w3.org/TR/selectors-4/#adjacent-sibling-combinators): `node + next`
    - [Subsequent-sibling](https://www.w3.org/TR/selectors-4/#general-sibling-combinators): `node ~ sibling`
- Pseudo-class
    - [Negation](https://www.w3.org/TR/selectors-4/#negation): `:not(...)`
    - [Matches-any](https://www.w3.org/TR/selectors-4/#matches): `:is(...)`
    - [Relational](https://www.w3.org/TR/selectors-4/#relational): `:has(...)`
    - [`:nth-child`](https://www.w3.org/TR/selectors-4/#the-nth-child-pseudo)
    - [`:first-child`](https://www.w3.org/TR/selectors-4/#the-first-child-pseudo)
    - [`:last-child`](https://www.w3.org/TR/selectors-4/#the-last-child-pseudo)
- Attributes (Properties)
    - `[PropName]` (existance)
    - `[PropName = 'Exact']`
    - `[PropName ^= 'StartsWith']`
    - `[PropName $= 'EndsWith']`
    - `[PropName *= 'Contains']`
    - `[PropName ~= 'Item']` (ex. `[Modifiers ~= 'async']`)
- Extensions
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
```