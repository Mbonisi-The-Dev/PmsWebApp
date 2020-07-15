using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PmsWebApi.Models;

namespace PmsWebApp.Controllers
{
    public class NotificationsListController : Controller
    {
        private string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("notifications");

                if (response.IsSuccessStatusCode)
                {
                    var maintenance = response.Content.ReadAsAsync<IEnumerable<NotificationsListController>>().Result;

                    return View(maintenance);
                }
                //HttpResponseMessage response = await client.GetAsync("api/tenant").Result;

                //var stringData =  response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var stringData = response.Content.ReadAsAsync<IEnumerable<TenantProfile>>().Result;

            }
            return View();
        }
    }
}