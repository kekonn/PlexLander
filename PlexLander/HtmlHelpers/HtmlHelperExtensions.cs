using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq.Expressions;

namespace PlexLander.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent ToggleSwitchFor<TModel>(this IHtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, bool disabled = false)
        {
            if (disabled)
                return html.CheckBoxFor(expression, new { @data_toggle = "toggle", disabled });
            else
                return html.CheckBoxFor(expression, new { @data_toggle = "toggle" });
        }
    }
}
