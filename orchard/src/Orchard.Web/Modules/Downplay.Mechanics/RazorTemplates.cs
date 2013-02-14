using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Downplay.Mechanics
{
    public static class RazorTemplates
    {
        public static IHtmlString Loop<T>(this IEnumerable<T> items, Func<T, object> itemTemplate, Func<T,object> separatorTemplate = null)
        {
            StringBuilder builder = new StringBuilder();
            var times = items.Count();
            var count = 0;
            foreach (var item in items)
            {
                // Item
                builder.Append(itemTemplate(item));
                count++;
                // Separator
                if (separatorTemplate!=null && count < times)
                {
                    builder.Append(separatorTemplate(item));
                }
            }
            return new HtmlString(builder.ToString());
        }
        /*
        public static IHtmlString LoopFor<TModel, TProperty>(this HtmlHelper html, Expression<Func<TModel, IEnumerable<TProperty>>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var inner = new HtmlHelper<TProperty>(html.ViewContext, html.ViewDataContainer, html.RouteCollection); // ?

html.ViewContext.
        }*/
    }
}