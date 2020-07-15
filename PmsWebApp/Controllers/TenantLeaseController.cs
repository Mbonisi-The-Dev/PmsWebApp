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
    public class TenantLeaseController : Controller
    {
        //private string BaseUrl = "https://pmswebapiv1.azurewebsites.net/api/";
        private readonly string BaseUrl = "	http://MBONISITSHUMA1.af.didata.local:9027/api/";

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Index(string IdNumber)
        {
            TenantLease  tenant = null;
            using (var client = new HttpClient()) 
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               // HttpResponseMessage response = await client.GetAsync($"tenantlease?idNumber={IdNumber}");

                var result = await client.GetAsync($"tenantlease/{IdNumber} ");
                if (result.IsSuccessStatusCode)
                {
                     tenant =  await result.Content.ReadAsAsync<TenantLease>();
                   // return View(tenant);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(tenant);
        }

        public async Task<ActionResult> Create(string IdNumber)
        {

            TenantLease tenant = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"tenantlease/{IdNumber}");

                //var result = await client.GetAsync($"tenant/{IdNumber} ");
                if (response.IsSuccessStatusCode)
                {
                    tenant = await response.Content.ReadAsAsync<TenantLease>();
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

        // POST: Tenant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TenantLease tenant)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Result = client.PostAsJsonAsync("tenantlease", tenant).Result;

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