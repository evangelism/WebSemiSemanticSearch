using Microsoft.Cognitive.LUIS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WSSS.Models;

namespace WSSS.Controllers
{
    public class SearchController : ApiController
    {
        /*
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */

        // GET api/<controller>/<search term>
        public async Task<SearchResult[]> Get(string q)
        {
            var LUIS = new LuisClient(Config.LUISModel, Config.LUISKey);
            var lr = await LUIS.Predict(q);
            if (lr.TopScoringIntent.Score>Config.LUISTreshold)
            {
                var fn = HttpContext.Current.Server.MapPath("../actions.json");
                var s = File.ReadAllText(fn);
                dynamic A = JsonConvert.DeserializeObject(s);
                foreach (dynamic x in A.actions)
                {
                    if (x.intent == lr.TopScoringIntent.Name)
                    {
                        if (Utils.PropertyExists(x, "redirect"))
                        {
                            return (new SearchResult(x.text.ToString(), x.redirect.ToString())).AsArray();
                        }
                        if (Utils.PropertyExists(x, "search"))
                        {
                            var sb = new StringBuilder();
                            int cnt = 0;
                            foreach (dynamic t in x.search)
                            {
                                var v = LookupEntity(lr.Entities, t.entity.ToString());
                                if (v != "")
                                {
                                    if (cnt++ > 0) sb.Append(" and ");
                                    sb.Append($"{t.field} eq '{v}'"); // TODO: Add UrlEncode here
                                }
                            }
                            if (cnt > 0)
                            {
                                var AzS = new AzSearch(Config.AzSearchUrl, Config.AzSearchKey);
                                dynamic rres = await AzS.Search("azuretable-index", sb.ToString());
                                var l = new List<SearchResult>();
                                foreach (var o in rres)
                                {
                                    l.Add(new SearchResult()
                                    {
                                        Text = $"{o.brand} - {o.model}",
                                        Url = "",
                                        Description = Utils.BuildDesc(o, new[] { "Storage", "Battery", "sim_type", "front_cam", "back_cam" })
                                    });
                                }
                                return l.ToArray();
                            } // otherwise fallback to bing search
                        }
                    }
                }
            }
            var CSearch = new BingCustomSearch(Config.CustomSearchKey, Config.CustomSearchID);
            var res = await CSearch.Search(q);
            return (from x in res.webPages.value
                    select new SearchResult(x)).ToArray();
        }

        private string LookupEntity(IDictionary<string, IList<Entity>> entities, string z)
        {
            foreach(var x in entities.Keys)
            {
                if (x.ToLower()==z.ToLower() || (x.ToLower().StartsWith(z.ToLower())))
                {
                    return entities[x][0].Value;
                }
            }
            return "";
        }

        /*
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        */
    }
}