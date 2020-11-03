using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Northwind.Web.Extensions
{
    public static class EnumerableExtensions
    {
        public static SelectList ToSelectList(this IEnumerable enumerable, string identifier, string itemName)
        {
            return new SelectList(enumerable, identifier, itemName);
        }
    }
}
