using Microsoft.AspNetCore.Mvc.Razor;

public class ViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context) { }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        return new[]
        {
            "/Pages/{0}.cshtml",
            "/Pages/Shared/{0}.cshtml"
        }.Union(viewLocations);
    }
}