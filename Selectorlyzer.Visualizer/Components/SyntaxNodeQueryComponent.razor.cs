using Microsoft.AspNetCore.Components;

namespace Selectorlyzer.Visualizer.Components
{
    public partial class SyntaxNodeQueryComponent
    {
        [Inject]
        public required ISelectedNodeContainer SelectedNodeContainer { get; set; }

        protected string? Expression => SelectedNodeContainer.Expression;

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
