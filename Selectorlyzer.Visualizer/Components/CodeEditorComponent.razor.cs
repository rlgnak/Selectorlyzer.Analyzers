using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Selectorlyzer.Qulaly;

namespace Selectorlyzer.Visualizer.Components
{
    public partial class CodeEditorComponent
    {
        [Inject]
        public required ICodeContainer CodeContainer { get; set; }

        [Inject]
        public required ISelectedNodeContainer SelectedNodeContainer { get; set; }

        [Inject]
        public required IQueryContainer QueryContainer { get; set; }

        protected StandaloneCodeEditor CodeEditorReference { get; set; } = null!;

        protected List<string>? CodeEditorHighlightDecorators { get; set; }

        private IEnumerable<ModelDeltaDecoration> MatchedDecorators { get; set; } = [];

        private ModelDeltaDecoration? SelectedNodeDecorator { get; set; }

        protected override void OnInitialized()
        {
            QueryContainer.OnChange += HighlightQulayQuery;
            SelectedNodeContainer.OnChange += HighlightSyntaxNode;
        }

        protected StandaloneEditorConstructionOptions EditorConstructionOptions(
            StandaloneCodeEditor editor
        )
        {
            return new StandaloneEditorConstructionOptions
            {
                Language = "csharp",
                Value = CodeContainer.Code,
                Contextmenu = false,
                Minimap = new EditorMinimapOptions { Enabled = false },
                SelectionHighlight = false,
                UnicodeHighlight = new UnicodeHighlightOptions
                {
                    AmbiguousCharacters = false,
                    IncludeStrings = false,
                    IncludeComments = false,
                    NonBasicASCII = false,
                    InvisibleCharacters = false,
                },
                FoldingHighlight = false,
                CopyWithSyntaxHighlighting = false,
            };
        }

        protected void EditorDidChangeCursorPosition(CursorPositionChangedEvent eventArgs)
        {
            var selectedNode = default(SyntaxNode);

            if (CodeContainer.CompilationUnit != null)
            {
                selectedNode = CodeContainer.CompilationUnit.SyntaxTree.FindNodeByLineColumn(
                    eventArgs.Position.LineNumber,
                    eventArgs.Position.Column
                );
            }

            SelectedNodeContainer.Selector = selectedNode;
        }

        protected async Task EditorOnDidInit()
        {
            CodeContainer.Code = await CodeEditorReference.GetValue();
        }

        private async Task EditorOnDidPaste(PasteEvent args)
        {
            CodeContainer.Code = await CodeEditorReference.GetValue();
        }

        protected async Task EditorOnModelContentChanged(ModelContentChangedEvent keyboardEvent)
        {
            CodeContainer.Code = await CodeEditorReference.GetValue();
        }

        protected async Task HighlightSyntaxNode()
        {
            SelectedNodeDecorator = HighlightSynatxNode(SelectedNodeContainer.Selector, "selected-node");

            var model = await CodeEditorReference.GetModel();
            CodeEditorHighlightDecorators = await model.DeltaDecorations(
                CodeEditorHighlightDecorators,
                [SelectedNodeDecorator, .. MatchedDecorators],
                default
            );
        }

        protected async Task HighlightQulayQuery()
        {
            IEnumerable<SyntaxNode> nodes;

            if (CodeContainer.CompilationUnit == null || QueryContainer.Selector == null)
            {
                nodes = [];
            }
            else
            {
                try
                {
                    nodes = CodeContainer.CompilationUnit
                        .SyntaxTree.GetRoot()
                        .QuerySelectorAll(QueryContainer.Selector);
                }
                catch
                {
                    nodes = [];
                }
            }

            MatchedDecorators = HighlightSynatxNodes(nodes, "highlight-", true);

            var model = await CodeEditorReference.GetModel();
            CodeEditorHighlightDecorators = await model.DeltaDecorations(
                CodeEditorHighlightDecorators,
                [SelectedNodeDecorator, .. MatchedDecorators],
                default
            );
        }

        protected ModelDeltaDecoration? HighlightSynatxNode(SyntaxNode? syntaxNode, string className)
        {
            if (syntaxNode == null)
            {
                return null;
            }

            try
            {
                var location = syntaxNode.GetLocation().GetLineSpan();
                var decorator = CreateHighlightModelDeltaDecoration(syntaxNode, className);
                return decorator;
            }
            catch
            {
                return null;
            }
        }

        protected List<ModelDeltaDecoration> HighlightSynatxNodes(IEnumerable<SyntaxNode> syntaxNodes, string className, bool appendIndex)
        {
            var matchedDecorators = new List<ModelDeltaDecoration>();

            var index = 1;
            try
            {

                foreach (var match in syntaxNodes)
                {
                    var decorator = HighlightSynatxNode(match, className + (appendIndex ? index++ % 12 : ""));
                    if (decorator != null)
                    {
                        matchedDecorators.Add(decorator);
                    }
                }
            }
            catch
            {
                matchedDecorators = [];
            }

            return matchedDecorators;
        }

        private ModelDeltaDecoration CreateHighlightModelDeltaDecoration(SyntaxNode syntaxNode, string className)
        {
            var location = syntaxNode.GetLocation().GetLineSpan();

            return new ModelDeltaDecoration
            {
                Range = new BlazorMonaco.Range
                {
                    StartLineNumber = location.StartLinePosition.Line + 1,
                    StartColumn = location.StartLinePosition.Character + 1,
                    EndColumn = location.EndLinePosition.Character + 1,
                    EndLineNumber = location.EndLinePosition.Line + 1,
                },
                Options = new ModelDecorationOptions
                {
                    IsWholeLine = false,
                    InlineClassName = className,
                },
            };
        }
    }
}
