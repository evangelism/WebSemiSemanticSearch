using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WSSS.Models;

namespace WSSS.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string Q)
        {
            var http = new HttpClient();
            var s = Request.Url + "api/search?q=";
            var ress = await http.GetStringAsync(s + HttpUtility.UrlEncode(Q));
            var res = JsonConvert.DeserializeObject<SearchResult[]>(ress);
            if (res.Length==1 && res[0].AutoRedirect)
            {
                return Redirect(res[0].Url);
            }
            return View(new SearchModel() { Results = res });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}