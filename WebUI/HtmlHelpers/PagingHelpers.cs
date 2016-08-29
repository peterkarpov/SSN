namespace WebUI.HtmlHelpers
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using WebUI.Models;

    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int, string> pageUrl)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");

                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    li.AddCssClass("active");
                }

                li.InnerHtml += tag.ToString();
                ul.InnerHtml += li.ToString();
            }

            result.Append(ul.ToString());
            return MvcHtmlString.Create(result.ToString());
        }
    }
}