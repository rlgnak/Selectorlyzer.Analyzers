using Microsoft.AspNetCore.Components;

public partial class HomeComponent : ComponentBase
{
    [Inject]
    public required IQueryContainer QueryContainer { get; set; }

    protected void OnInputQuery(string query)
    {
        QueryContainer.Selector = query;
    }
}
