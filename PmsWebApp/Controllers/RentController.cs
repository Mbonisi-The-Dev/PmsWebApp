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
    public class RentController : Controller
    {

        //private string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";
        private string BaseUrl = "http://mbonisitshuma1.af.didata.local:8517/";

        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("rent");

                if (response.IsSuccessStatusCode)
                {
                    var rent = response.Content.ReadAsAsync<IEnumerable<Rent>>().Result;

                    return View(rent);
                }       
            }
            return View();
        }


        public async Task<ActionResult> Details(string InvoiceNumber )
        {
            Rent rent = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var result = await client.GetAsync($"rent/{InvoiceNumber} ");

                if (result.IsSuccessStatusCode)
                {
                    rent = await result.Content.ReadAsAsync<Rent>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                // var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var dataList = stringData.Where( x => x.UserId == UserId);
                //return View(dataList.ToList());
            }

            return View(rent);

        }

    }
}