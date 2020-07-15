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
using System.ComponentModel.DataAnnotations;


namespace PmsWebApp.Controllers
{
    public class TenantController : Controller
    {

        //private readonly string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";
        private readonly string BaseUrl = "	http://MBONISITSHUMA1.af.didata.local:9027/api/";
        public async Task<ActionResult> Index(Object uObj)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("tenant");

                if (response.IsSuccessStatusCode)
                {
                    var tenants = response.Content.ReadAsAsync<IEnumerable<Tenant>>().Result;

                    var role = HttpContext.Session.GetInt32("UserRole");

                    if (role == 5)
                    {
                        var oneTenant = response.Content.ReadAsAsync<IList<Tenant>>().Result;
                        //var tenantObj = oneTenant.Where(u => u.Equals())
                        return RedirectToAction("Create", "Tenant");
                        //return View();
                    }
                    else
                    {
                        return View(tenants);
                    }

                }
                //HttpResponseMessage response = await client.GetAsync("api/tenant").Result;

                //var stringData =  response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var stringData = response.Content.ReadAsAsync<IEnumerable<TenantProfile>>().Result;

            }
            return View();
        }

        public async Task<ActionResult> Details(int UserId)
        {
            Tenant tenant = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var result = await client.GetAsync($"tenant/{UserId} ");

                if (result.IsSuccessStatusCode)
                {
                    tenant = await result.Content.ReadAsAsync<Tenant>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                // var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var dataList = stringData.Where( x => x.UserId == UserId);
                //return View(dataList.ToList());
            }

            return View(tenant);

        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(TenantController obj)
        {
            using (var client = new HttpClient())
            {
                string stringData = JsonConvert.SerializeObject(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("/tenant", contentData).Result;
                ViewBag.Message = response.Content.ReadAsStringAsync().Result;
                return View(obj);
            }
        }

        // POST: Tenant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tenant tenant)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                int userId = (int)HttpContext.Session.GetInt32("UserId");
                tenant.UserId = userId;
                Object tObj = new { };

                var Result = client.PostAsJsonAsync("tenant", tenant).Result;

                if (Result.IsSuccessStatusCode == true)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View();
                }
            }
        }

        //Get Tenant/Edit/12345
        public async Task<ActionResult> Edit(int? UserId)
        {
            Tenant tenant = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                //HttpResponseMessage response = await client.GetAsync("tenant/{UserId}");
                //var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;

                HttpResponseMessage response = await client.GetAsync($"tenant/{UserId}");

                if (response.IsSuccessStatusCode)
                {
                    tenant = await response.Content.ReadAsAsync<Tenant>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                //var dataList = stringData.Where(x => x.UserId == UserId);
                //return View(dataList.ToList());
            }
            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int UserId, Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PutAsJsonAsync($"tenant/{UserId}", tenant);

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

            return View(tenant);
        }

        public async Task<ActionResult> Delete(int UserId)
        {
            Tenant tenant = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var result = await client.GetAsync($"tenant/{UserId}");

                if (result.IsSuccessStatusCode)
                {
                    tenant = await result.Content.ReadAsAsync<Tenant>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(tenant);
        }

        // POST: TaskDetails/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int UserId, Tenant tenant)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync($"tenant/{UserId}");
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

