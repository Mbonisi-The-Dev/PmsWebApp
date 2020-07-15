using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PmsWebApi.Models;

namespace PmsWebApp.Controllers
{
    public class MaintenanceController : Controller
    {
        private string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";

        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("maintenance");

                if (response.IsSuccessStatusCode)
                {
                    var maintenance = response.Content.ReadAsAsync<IEnumerable<Maintenance>>().Result;

                    return View(maintenance);
                }
                //HttpResponseMessage response = await client.GetAsync("api/tenant").Result;

                //var stringData =  response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var stringData = response.Content.ReadAsAsync<IEnumerable<TenantProfile>>().Result;

            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Tenant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Maintenance maintenance)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Result = client.PostAsJsonAsync("maintenance",  maintenance).Result;

                if (Result.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    return View();
                }
            }
        }

        public async Task<ActionResult> Details(string LogId)
        {
            Maintenance maintenance = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var result = await client.GetAsync($"maintenance/{LogId} ");

                if (result.IsSuccessStatusCode)
                {
                    maintenance = await result.Content.ReadAsAsync<Maintenance>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                // var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var dataList = stringData.Where( x => x.UserId == UserId);
                //return View(dataList.ToList());
            }
            return View(maintenance);
        }

        public async Task<ActionResult> Edit(string LogId)
        {
            Maintenance maintenance = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                //HttpResponseMessage response = await client.GetAsync("tenant/{UserId}");
                //var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;

                HttpResponseMessage response = await client.GetAsync($"maintenance/{LogId}");

                if (response.IsSuccessStatusCode)
                {
                    maintenance = await response.Content.ReadAsAsync<Maintenance>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                //var dataList = stringData.Where(x => x.UserId == UserId);
                //return View(dataList.ToList());
            }
            return View(maintenance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string LogId, Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PutAsJsonAsync($"maintenance/{LogId}", maintenance);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(maintenance);
        }

        public async Task<ActionResult> Delete(string LogId)
        {
            Maintenance maintenance = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync($"maintenance/{LogId}");

                if (result.IsSuccessStatusCode)
                {
                    maintenance = await result.Content.ReadAsAsync<Maintenance> ();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(maintenance);
        }

        // POST: TaskDetails/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string LogId, Maintenance maintenance)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync($"maintenance/{LogId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

    }
}