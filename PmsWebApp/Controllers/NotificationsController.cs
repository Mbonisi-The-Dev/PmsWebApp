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
    public class NotificationsController : Controller
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
                    var notifications = response.Content.ReadAsAsync<IEnumerable<Notifications>>().Result;

                    return View(notifications);
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
        public IActionResult Create(Notifications notifications)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Result = client.PostAsJsonAsync("notifications", notifications).Result;

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

        public async Task<ActionResult> Details(string NoteId)
        {
            Notifications notifications = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var result = await client.GetAsync($"notifications/{NoteId} ");

                if (result.IsSuccessStatusCode)
                {
                    notifications = await result.Content.ReadAsAsync<Notifications>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                // var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var dataList = stringData.Where( x => x.UserId == UserId);
                //return View(dataList.ToList());
            }
            return View(notifications);
        }

        public async Task<ActionResult> Edit(string NoteId)
        {
            Notifications notifications = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                //HttpResponseMessage response = await client.GetAsync("tenant/{UserId}");
                //var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;

                HttpResponseMessage response = await client.GetAsync($"notifications/{NoteId}");

                if (response.IsSuccessStatusCode)
                {
                    notifications = await response.Content.ReadAsAsync<Notifications>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                //var dataList = stringData.Where(x => x.UserId == UserId);
                //return View(dataList.ToList());
            }
            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string NoteId, Notifications notifications)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PutAsJsonAsync($"notifications/{NoteId}", notifications);

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
            return View(notifications);
        }

        public async Task<ActionResult> Delete(string NoteId)
        {
            Notifications notifications = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.GetAsync($"notifications/{NoteId}");

                if (result.IsSuccessStatusCode)
                {
                    notifications = await result.Content.ReadAsAsync<Notifications>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(notifications);
        }

        // POST: TaskDetails/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string NoteId, Notifications notifications)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync($"notifications/{NoteId}");
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