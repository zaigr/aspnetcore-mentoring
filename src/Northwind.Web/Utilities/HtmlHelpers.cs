using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Northwind.Web.Utilities
{
    public static class HtmlHelpers
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper helper, int imageId, string content)
        {
            return new HtmlString($"<a href=\"/images/{imageId}\">{content}</a>");
        }
    }
}
