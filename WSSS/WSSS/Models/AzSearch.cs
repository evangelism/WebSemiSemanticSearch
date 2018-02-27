using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WSSS.Models
{
    public class AzSearch
    {
        public string Key { get; set; }
        public string Url { get; set; }

        public AzSearch(string url, string key)
        {
            Key = key; Url = url;
        }

        public async Task<dynamic> Search(string table, string q)
        {
            var r = $"{Url}/indexes/{table}/docs?api-version=2016-09-01&api-key={Key}&search=" + HttpUtility.UrlEncode(q);
            var http = new HttpClient();
            var res = await http.GetStringAsync(r);
            dynamic o = JsonConvert.DeserializeObject(res);
            return o.value;
        }

    }
}