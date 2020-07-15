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
    public class TenantLeaseRenewalController : Controller
    {
        //private string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";
        private readonly string BaseUrl = "	http://MBONISITSHUMA1.af.didata.local:9027/api/";

        //public async Task<ActionResult> Index(string LeaseNumber)
        //{
        //    TenantLeaseRenewal tenant = null;

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BaseUrl);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response = await client.GetAsync($"tenantleaserenewal?LeaseNumber={LeaseNumber} ");

        //        //var result = await client.GetAsync($"tenantleaserenewal?LeaseNumber={LeaseNumber} ");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            tenant = await response.Content.ReadAsAsync<TenantLeaseRenewal>();
        //            //return View();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Server error try after some time.");
        //        }
        //    }
        //    return View(tenant);
        //}

        public async Task<ActionResult> Index(string LeaseNumber)
        {
            //TenantLeaseRenewal tenantLeaseRenewal = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = await client.GetAsync($"tenantleaserenewal?LeaseNumber={LeaseNumber} ");

                var result = await client.GetAsync($"tenantleaserenewal?LeaseNumber={LeaseNumber} ");

                if (result.IsSuccessStatusCode)
                {
                    var tenantLeaseRenewal = result.Content.ReadAsAsync<IList<TenantLeaseRenewal>>().Result;

                    var filteredList = tenantLeaseRenewal.Where( x => x.LeaseNumber == LeaseNumber).ToList();
                  
                    return View(filteredList);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View();
        }

        public async Task<ActionResult> Create(string LeaseNumber)
        {

            TenantLeaseRenewal tenant = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"tenantleaserenewal/{LeaseNumber}");

                //var result = await client.GetAsync($"tenant/{IdNumber} ");
                if (response.IsSuccessStatusCode)
                {
                    tenant = await response.Content.ReadAsAsync<TenantLeaseRenewal>();
                    // return View(tenant);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(tenant);
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TenantLeaseRenewal tenantLeaseTermination)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Result = client.PostAsJsonAsync("tenantleaserenewal", tenantLeaseTermination).Result;

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
    }
}