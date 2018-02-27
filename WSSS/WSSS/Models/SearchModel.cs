using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSSS.Models
{
    public class SearchModel
    {
        public SearchResult[] Results;
    }

    public class SearchResult
    {
        public SearchResult()
        {
        }

        // Return search result that does redirect
        public SearchResult(string text, string url, bool auto = true)
        {
            this.Url = url;
            this.Text = text;
            this.AutoRedirect = auto;
        }

        public SearchResult(WebPage x)
        {
            Url = x.url;
            Text = x.name;
            Description = x.snippet;
            ImageUrl = x.openGraphImage?.contentUrl;
        }

        public string Url { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; } = null;
        public string Description { get; set; }
        public bool AutoRedirect { get; set; } = false;
    }

}