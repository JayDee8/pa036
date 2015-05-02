using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Text;

namespace bcpp.Helpers
{
    public static class Html
    {
        /// <summary>
        /// Displays the sort direction arrow depending on a grids current column state.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="grid"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static MvcHtmlString SortDirection(this HtmlHelper helper, ref WebGrid grid, String columnName)
        {
            String html = "";

            if (grid.SortColumn == columnName && grid.SortDirection == System.Web.Helpers.SortDirection.Ascending)
            {
                html = "˅";
            }
            else if (grid.SortColumn == columnName && grid.SortDirection == System.Web.Helpers.SortDirection.Descending)
            {
                html = "˄";
            }
            else
            {
                html = "";
            }

            return MvcHtmlString.Create(html);
        }
        
        public static MvcHtmlString Nl2Br(this HtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return MvcHtmlString.Create(text);
            else
            {
                StringBuilder builder = new StringBuilder();
                string[] lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        builder.Append("<br/>\n");
                    builder.Append(HttpUtility.HtmlEncode(lines[i]));
                }
                return MvcHtmlString.Create(builder.ToString());
            }
        }


    }

    public static class GridExtensions
    {
        public static WebGridColumn RoleBasedColumns(this HtmlHelper htmlHelper, string role, WebGrid grid, string columnKey, string columnName)
        {
            var user = htmlHelper.ViewContext.HttpContext.User;
            var column = new WebGridColumn();

            // The Prop1 column would be visible to all users
            //columns.Add(grid.Column("Prop1"));

            if (user.IsInRole(role))
            {
                // The Prop2 column would be visible only to users
                // in the foo role
                column = grid.Column(columnName,columnKey);
            }
            return column;
        }

        public static WebGridColumn RoleBasedLinks(this HtmlHelper htmlHelper, string role, WebGrid grid, string actionKey, string columnName)
        {
            //dodělat!!!!!!!!!!!!!!!
            var user = htmlHelper.ViewContext.HttpContext.User;
            var column = new WebGridColumn();

            // The Prop1 column would be visible to all users
            //columns.Add(grid.Column("Prop1"));

            if (user.IsInRole(role))
            {
                // The Prop2 column would be visible only to users
                // in the foo role
                //column.Add(grid.Column(columnName, columnKey));
               // column = Html.ActionLink("Smazat", "Delete", new { id = item.pk_id });
            }
            return column;
        }
    }
}