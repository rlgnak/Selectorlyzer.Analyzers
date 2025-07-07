using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace Selectorlyzer.Visualizer.Components
{
    public partial class VirtualTreeView<T> where T : class
    {
        protected string ListItemClassname() => new CssBuilder("mud-treeview-item").Build();

        protected string ContentClassname(bool isSelected) =>
            new CssBuilder("mud-treeview-item-content")
                .AddClass("cursor-pointer")
                .AddClass("mud-treeview-item-selected", isSelected)
                .Build();

        protected string TreeViewClassname() => new CssBuilder("mud-treeview").Build();

        private readonly record struct ViewItem(T Item, string Key, bool IsExpanded, int Level);

        private List<ViewItem> viewList = new();

        private List<T>? items;

        private T? selectedValue;

        [Parameter]
        public RenderFragment<T>? ItemTemplate { get; set; }

        [Parameter]
        public required List<T> Items { get; set; }

        [Parameter]
        public required Func<T, List<T>> ChildrenSelector { get; set; }

        [Parameter]
        public T? SelectedValue { get; set; }

        [Parameter]
        public required Func<T, T?> ParentSelector { get; set; }

        [Parameter]
        public EventCallback<T?> SelectedValueChanged { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Items != items)
            {
                items = Items;
                SynchronizeViewList();
            }

            if (SelectedValue != selectedValue)
            {
                selectedValue = SelectedValue;

                if (selectedValue != null)
                {
                    var parent = ParentSelector(selectedValue);

                    if (parent != null)
                    {
                        ExpandedTo(parent);
                    }
                }
                else
                {
                    // lame hack to reset if value is null
                    SynchronizeViewList();
                }
            }
        }

        private void ExpandedTo(T value)
        {
            // lame hack to collapse any children of the current selected node
            SynchronizeViewList();

            var parent = ParentSelector(value);

            if (parent != null)
            {
                ExpandedTo(parent);
            }

            var item = viewList.FirstOrDefault(x => x.Item == value);
            if (!item.IsExpanded)
            {
                OnClick(true, item);
            }
        }

        private void SynchronizeViewList()
        {
            viewList.Clear();
            if (Items is { } items)
            {
                viewList.AddRange(items.Select((x, i) => new ViewItem(x, $"{i}", false, 0)));
            }
        }

        private async Task onSelect(ViewItem item)
        {
            SelectedValue = item.Item;

            if (SelectedValueChanged.HasDelegate)
            {
                await SelectedValueChanged.InvokeAsync(SelectedValue);
            }
        }

        private void OnClick(bool newIsExpanded, ViewItem item)
        {
            var viewListIndex = viewList.FindIndex(x =>
                EqualityComparer<ViewItem>.Default.Equals(x, item)
            );

            if (viewListIndex < 0)
            {
                return;
            }

            viewList[viewListIndex] = item with { IsExpanded = newIsExpanded };
            var children = ChildrenSelector?.Invoke(item.Item) ?? [];
            if (newIsExpanded)
            {
                viewList.InsertRange(
                    viewListIndex + 1,
                    children.Select(
                        (x, i) => new ViewItem(x, $"{item.Key}-{i}", false, item.Level + 1)
                    )
                );
            }
            else
            {
                var cnt = children.Count;
                cnt += viewList
                    .Skip(viewListIndex + 1 + cnt)
                    .TakeWhile(x => x.Key.StartsWith(item.Key))
                    .Count();
                viewList.RemoveRange(viewListIndex + 1, cnt);
            }
        }
    }
}
