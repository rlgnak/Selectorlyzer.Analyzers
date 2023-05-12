using Selectorlyzer.LightJson;
using System.Collections.Generic;

namespace Selectorlyzer.Analyzers;

public class SelectorlyzerConfig
{
    public SelectorlyzerConfig()
    {
    }

    public SelectorlyzerConfig(JsonObject configObject)
    {
        var rules = configObject["rules"].AsJsonArray;
        foreach (var rule in rules) {
            Rules.Add(new SelectorlyzerRule
            {
                Rule = rule["rule"].AsString,
                Message = rule["message"].AsString,
                Selector = rule["selector"].AsString,
                Severity = rule["severity"].AsString,
            });
        }
    }

    public List<SelectorlyzerRule>? Rules { get; set; } = new List<SelectorlyzerRule> ();

    public JsonObject ToJsonObject()
    {
        var json = new JsonObject();

        if (Rules == null)
        {
            return json;
        }

        var rules = new JsonArray();
        foreach(var rule in Rules)
        {
            var jsonRule = new JsonObject
            {
                { "message", rule.Message },
                { "selector", rule.Selector! }
            };

            if (rule.Severity != null)
            {
                jsonRule["severity"] = rule.Severity;
            }

            if (rule.Rule != null)
            {
                jsonRule["rule"] = rule.Rule;
            }

            rules.Add(jsonRule);
        }

        json["rules"] = rules;

        return json;
    }
}
