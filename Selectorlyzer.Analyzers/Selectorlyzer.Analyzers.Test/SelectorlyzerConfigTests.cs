using FluentAssertions;
using Selectorlyzer.LightJson.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Selectorlyzer.Analyzers.Test
{
    public class SelectorlyzerConfigTests
    {
        [Fact]
        public void Should_be_able_to_parse_rules()
        {
            var json = @"
{
  ""rules"": [
    {
      ""selector"": "":class"",
      ""message"": ""class error message"",
      ""severity"": ""error""
    },
    {
      ""selector"": "":namespace"",
      ""rule"": "":class"",
      ""message"": ""namespace error message"",
      ""severity"": ""warning""
    }
  ]
}
";
            var result = new SelectorlyzerConfig(JsonReader.Parse(json));

            result.Rules.Should().HaveCount(2);

            result.Rules![0].Selector.Should().Be(":class");
            result.Rules![0].Message.Should().Be("class error message");
            result.Rules![0].Rule.Should().BeNull();

            result.Rules![1].Selector.Should().Be(":namespace");
            result.Rules![1].Message.Should().Be("namespace error message");
            result.Rules![1].Rule.Should().Be(":class");
        }
    }
}
