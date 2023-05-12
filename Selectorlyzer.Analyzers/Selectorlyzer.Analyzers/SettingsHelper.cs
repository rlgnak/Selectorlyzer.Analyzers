using Microsoft.CodeAnalysis.Diagnostics;
using Selectorlyzer.LightJson.Serialization;
using System;
using System.IO;
using System.Threading;

namespace Selectorlyzer.Analyzers
{
    public static class SettingsHelper
    {
        public const string SettingsFileName = "selectorlyzer.json";
        public const string AltSettingsFileName = ".selectorlyzer.json";

        public static bool IsConfigFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var fileName = Path.GetFileName(path);

            return string.Equals(fileName, SettingsFileName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(fileName, AltSettingsFileName, StringComparison.OrdinalIgnoreCase);
        }

        public static SelectorlyzerConfig? GetConfig(AnalyzerOptions options, CancellationToken cancellationToken)
        {
            if (options == null)
            {
                return null;
            }

            foreach (var additionalFile in options.AdditionalFiles)
            {
                if (IsConfigFile(additionalFile.Path))
                {
                    var additionalTextContent = additionalFile.GetText(cancellationToken);

                    if (additionalTextContent == null)
                    {
                        continue;
                    }

                    var result = new SelectorlyzerConfig(JsonReader.Parse(additionalTextContent.ToString()));

                    if (result != null) 
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
