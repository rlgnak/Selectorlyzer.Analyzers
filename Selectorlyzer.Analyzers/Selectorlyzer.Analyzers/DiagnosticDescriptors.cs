using Microsoft.CodeAnalysis;

namespace Selectorlyzer.Analyzers
{


    public static class DiagnosticDescriptors
    {
        internal const string WarningId = "SEL0001";
        internal static readonly LocalizableString WarningTitle = new LocalizableResourceString(nameof(Resources.AnalyzerWarningTitle), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString WarningMessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerWarningMessageFormat), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString WarningDescription = new LocalizableResourceString(nameof(Resources.AnalyzerWarningDescription), Resources.ResourceManager, typeof(Resources));

        internal const string ErrorId = "SEL0002";
        internal static readonly LocalizableString ErrorTitle = new LocalizableResourceString(nameof(Resources.AnalyzerErrorTitle), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString ErrorMessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerErrorMessageFormat), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString ErrorDescription = new LocalizableResourceString(nameof(Resources.AnalyzerErrorDescription), Resources.ResourceManager, typeof(Resources));

        internal const string InfoId = "SEL0003";
        internal static readonly LocalizableString InfoTitle = new LocalizableResourceString(nameof(Resources.AnalyzerInfoTitle), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString InfoMessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerInfoMessageFormat), Resources.ResourceManager, typeof(Resources));
        internal static readonly LocalizableString InfoDescription = new LocalizableResourceString(nameof(Resources.AnalyzerInfoDescription), Resources.ResourceManager, typeof(Resources));


        public static readonly DiagnosticDescriptor SelectorlyzerWarning = new DiagnosticDescriptor(
            WarningId,
            WarningTitle,
            WarningMessageFormat,
            "Convention",
            DiagnosticSeverity.Warning,
            true,
            WarningDescription
        );

        public static readonly DiagnosticDescriptor SelectorlyzerError = new DiagnosticDescriptor(
            ErrorId,
            ErrorTitle,
            ErrorMessageFormat,
            "Convention",
            DiagnosticSeverity.Error,
            true,
            ErrorDescription
        );

        public static readonly DiagnosticDescriptor SelectorlyzerInfo = new DiagnosticDescriptor(
            InfoId,
            InfoTitle,
            InfoMessageFormat,
            "Convention",
            DiagnosticSeverity.Info,
            true,
            InfoDescription
        );
    }
}