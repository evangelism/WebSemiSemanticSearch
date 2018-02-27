using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WSSS.Models
{
    public class BingCustomSearch
    {
        private string subscriptionKey;
        private string customConfigId;

        public BingCustomSearch(string subscriptionKey, string customConfigId)
        {
            this.subscriptionKey = subscriptionKey;
            this.customConfigId = customConfigId;
        }

        public async Task<BingCustomSearchResponse> Search(string q)
        {
            var url = "https://api.cognitive.microsoft.com/bingcustomsearch/v7.0/search?" +
                    "q=" + HttpUtility.UrlEncode(q) +
                    "&customconfig=" + customConfigId;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            var httpResponseMessage = await client.GetAsync(url);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            BingCustomSearchResponse response = JsonConvert.DeserializeObject<BingCustomSearchResponse>(responseContent);
            return response;
        }
    }

    public class BingCustomSearchResponse
    {
        public string _type { get; set; }
        public WebPages webPages { get; set; }
    }

    public class WebPages
    {
        public string webSearchUrl { get; set; }
        public int totalEstimatedMatches { get; set; }
        public WebPage[] value { get; set; }
    }

    public class WebPage
    {
        public string name { get; set; }
        public string url { get; set; }
        public string displayUrl { get; set; }
        public string snippet { get; set; }
        public DateTime dateLastCrawled { get; set; }
        public string cachedPageUrl { get; set; }
        public OpenGraphImage openGraphImage { get; set; }
    }

    public class OpenGraphImage
    {
        public string contentUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}