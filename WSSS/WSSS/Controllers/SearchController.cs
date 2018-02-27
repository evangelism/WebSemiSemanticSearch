using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            var CSearch = new BingCustomSearch(Config.CustomSearchKey, Config.CustomSearchID);
            var res = await CSearch.Search(q);
            return (from x in res.webPages.value
                    select new SearchResult(x)).ToArray();
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