using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;

namespace Selectorlyzer.Visualizer.Components
{
    public partial class SyntaxNodePropertyListComponent
    {
        [Inject]
        public required ISelectedNodeContainer SelectedNodeContainer { get; set; }

        protected SyntaxNode? SelectedValue => SelectedNodeContainer.Selector;

        protected IEnumerable<PropertyInfo>? SelectorProperties => SelectedNodeContainer.SelectorProperties;


        protected override void OnInitialized()
        {
            SelectedNodeContainer.OnChange += UpdateExpression;
        }

        private Task UpdateExpression()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
