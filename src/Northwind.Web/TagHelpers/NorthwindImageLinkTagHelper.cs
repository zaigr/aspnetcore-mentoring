using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Northwind.Web.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class NorthwindImageLinkTagHelper : TagHelper
    {
        public int NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TagName != "a")
            {
                throw new InvalidOperationException($"Tag helper '{nameof(NorthwindImageLinkTagHelper)}' can only be used for '<a>' tag.");
            }

            output.Attributes.SetAttribute("href", $"/images/{NorthwindId}");
        }
    }
}
