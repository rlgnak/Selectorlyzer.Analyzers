using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;

namespace Selectorlyzer.Visualizer.Components
{
    public partial class SyntaxTreeComponent
    {
        [Inject]
        public required ISelectedNodeContainer SelectedNodeContainer { get; set; }

        [Inject]
        public required ICodeContainer CodeContainer { get; set; }

        protected SyntaxNode? SelectedValue => SelectedNodeContainer.Selector;

        protected List<SyntaxNode> RootList => CodeContainer.CompilationUnit?.SyntaxTree.GetRoot().ChildNodes().ToList() ?? [];

        protected override async Task OnInitializedAsync()
        {
            CodeContainer.OnChange += UpdateRootListAsync;
            SelectedNodeContainer.OnChange += UpdateRootListAsync;
        }

        private Task UpdateRootListAsync()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected void OnItemClicked(SyntaxNode selectedValue)
        {
            SelectedNodeContainer.Selector = selectedValue;
        }
    }
}