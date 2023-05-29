using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Selectorlyzer.Qulaly;
using Selectorlyzer.Qulaly.Matcher.Selectors;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Selectorlyzer.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SelectorlyzerDiagnosticAnalyzer : DiagnosticAnalyzer
{
    private static readonly Action<CompilationStartAnalysisContext> CompilationStartAction = HandleCompilationStart;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        DiagnosticDescriptors.SelectorlyzerWarning,
        DiagnosticDescriptors.SelectorlyzerError,
        DiagnosticDescriptors.SelectorlyzerInfo
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(CompilationStartAction);
    }

    private static void HandleCompilationStart(CompilationStartAnalysisContext context)
    {
        var config = SettingsHelper.GetConfig(context.Options, context.CancellationToken);

        if (config is null)
        {
            throw new NullReferenceException("Config not loaded.");
        }

        if (config.Rules is null)
        {
            return;
        }

        ProcessRules(context, config.Rules);
    }

    private static void ProcessRules(CompilationStartAnalysisContext context, List<SelectorlyzerRule> rules)
    {
        foreach (var rule in rules)
        {
            ProcessRule(context, rule);
        }
    }

    private static void ProcessRule(CompilationStartAnalysisContext context, SelectorlyzerRule selectorlyzerRule)
    {
        var selector = selectorlyzerRule.Selector;
        var rule = selectorlyzerRule.Rule;
        var message = selectorlyzerRule.Message ?? "Undefined Message";
        var severity = selectorlyzerRule.Severity ?? "Warning";

        if (selector == null)
        {
            throw new NullReferenceException("selectors can not be null.");
        }

        var qulalySelector = QulalySelector.Parse(selector);
        var analyzer = new Analyzer(qulalySelector, rule, message, severity, selectorlyzerRule.IncludeGeneratedCode);

        if (qulalySelector.Selector is TypeSelector typeSelector)
        {
            context.RegisterSyntaxNodeAction(analyzer.SyntaxNodeRule, typeSelector.Kind);
        }
        else
        {
            context.RegisterSyntaxTreeAction(analyzer.SyntaxTreeAnalysisRule);
        }
    }

    internal sealed class Analyzer
    {
        private readonly QulalySelector selector;
        private readonly string? rule;
        private readonly string message;
        private readonly string severity;
        private readonly bool includeGeneratedCode;

        public Analyzer(QulalySelector selector, string? rule, string message, string severity, bool includeGeneratedCode)
        {
            this.selector = selector;
            this.rule = rule;
            this.message = message;
            this.severity = severity;
            this.includeGeneratedCode = includeGeneratedCode;
        }

        public void SyntaxTreeAnalysisRule(SyntaxTreeAnalysisContext context)
        {
            if (includeGeneratedCode && context.IsGeneratedCode)
            {
                return;
            }

            foreach (var node in context.Tree.QuerySelectorAll(selector))
            {
                if (CheckRule(rule, node))
                {
                    continue;
                }

                var diagnostic = Diagnostic.Create(GetDiagnosticDescriptor(severity), node.GetLocation(), message);
                context.ReportDiagnostic(diagnostic);
            }
        }

        public void SyntaxNodeRule(SyntaxNodeAnalysisContext context)
        {
            if (includeGeneratedCode && context.IsGeneratedCode)
            {
                return;
            }

            var syntax = context.Node;

            var node = syntax?.QuerySelector(selector);           

            if (node is null)
            {
                return;
            }

            if (CheckRule(rule, node))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(GetDiagnosticDescriptor(severity), node.GetLocation(), message);
            context.ReportDiagnostic(diagnostic);
        }

        internal static bool CheckRule(string? rule, SyntaxNode node)
        {
            if (rule is null)
            {
                return false;
            }

            var selector = ReplacePlaceholders(node, rule);

            return node.QuerySelector(selector) is not null;
        }

        internal static DiagnosticDescriptor GetDiagnosticDescriptor(string severity)
        {
            if (severity.Equals("Error", StringComparison.OrdinalIgnoreCase))
            {
                return DiagnosticDescriptors.SelectorlyzerError;
            }

            if (severity.Equals("Info", StringComparison.OrdinalIgnoreCase))
            {
                return DiagnosticDescriptors.SelectorlyzerInfo;
            }

            return DiagnosticDescriptors.SelectorlyzerWarning;
        }

        internal static string ReplacePlaceholders(SyntaxNode node, string selector)
        {
            if (!selector.Contains("{"))
            {
                return selector;
            }

            var placeholders = new Dictionary<string, string>();

            switch (node)
            {
                case ClassDeclarationSyntax classDeclarationSyntax:
                    placeholders.Add("Name", classDeclarationSyntax.Identifier.Text);
                    break;
                case MethodDeclarationSyntax methodDeclarationSyntax:
                    placeholders.Add("Name", methodDeclarationSyntax.Identifier.Text);
                    break;
                case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                    placeholders.Add("Name", interfaceDeclarationSyntax.Identifier.Text);
                    break;
                case PropertyDeclarationSyntax propertyDeclarationSyntax:
                    placeholders.Add("Name", propertyDeclarationSyntax.Identifier.Text);
                    break;
            }

            return placeholders.Aggregate(selector, (args, pair) =>
                args.Replace($"{{{pair.Key}}}", pair.Value)
            );
        }
    }
}