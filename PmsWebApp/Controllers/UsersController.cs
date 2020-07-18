using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PmsWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PmsWebApp.Controllers
{
    public class UsersController : Controller
    {
        //private readonly string BaseUrl="https://localhost:44359/api/";
        //private readonly string BaseUrl = "https://pmswebapi-dev.azurewebsites.net/api/";

        private readonly string BaseUrl = "http://mbonisitshuma1.af.didata.local:8162/api/";  

        public async Task<ActionResult> Index()
        {
            //Users users = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("users");
                //HTTP GET
                //var result = await client.GetAsync("users");                
                //var stringData = response.Content.ReadAsAsync<IList<Users>>().Result;
                // var dataList = stringData.Where( x => x.UserName == UserName) ;

                if (response.IsSuccessStatusCode)
                {
                    var users = response.Content.ReadAsAsync<IEnumerable<Users>>().Result;
                    return View(users);

                }
                //else //web api sent error response 
                //{
                //    log response status here..users = Enumerable.Empty<Models.UserProfile>();

                //    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                //}
            }
            return View();
        }

        public ActionResult NullUsers()
        {
            return View();
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {

            ViewBag.UserRole = 5;
            return View();
        }

        // POST: Tenant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Users users)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Result = client.PostAsJsonAsync("Users", users).Result;

                if (Result.IsSuccessStatusCode == true)
                {
                    return RedirectToAction("Index", "Tenant");

                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string UserName, string Password, Users users)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync($"users?UserName={UserName}&Password={Password}");

                    if (result.IsSuccessStatusCode)
                    {
                        var usersList = result.Content.ReadAsAsync<IList<Users>>().Result;
                        var uObj = usersList.Where(u => u.UserName.Equals(users.UserName) && u.Password.Equals(users.Password)).FirstOrDefault();

                        if (uObj != null)
                        {

                            if (uObj.UserRole == 1)
                            {
                                HttpContext.Session.SetString("Username", uObj.UserName);
                                return RedirectToAction("Index", "Dashboard", uObj);
                            }
                            else
                            {
                                HttpContext.Session.SetString("Username", uObj.UserName);
                                return RedirectToAction("UserDashboard", uObj);
                            }



                            //HttpContext.Session.SetString("UserId", uObj.UserId );
                            //ISession["UserID"] = uObj.UserId.ToString();
                            //Session["UserName"] = uObj.UserName.ToString();



                        }
                        else
                        {
                            return RedirectToAction("Login");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View();
        }

        public ActionResult UserDashboard()
        {
            return View();
        }

        public async Task<ActionResult> AdminDashboard()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("users");

                if (response.IsSuccessStatusCode)
                {
                    var users = response.Content.ReadAsAsync<IEnumerable<Users>>().Result;

                    return View(users);
                }
                //HttpResponseMessage response = await client.GetAsync("api/tenant").Result;

                //var stringData =  response.Content.ReadAsAsync<IList<TenantProfile>>().Result;
                //var stringData = response.Content.ReadAsAsync<IEnumerable<TenantProfile>>().Result;

            }
            return View();
        }

        public async Task<ActionResult> Edit(int? UserId)
        {
            Users users = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                //HttpResponseMessage response = await client.GetAsync("tenant/{UserId}");
                //var stringData = response.Content.ReadAsAsync<IList<TenantProfile>>().Result;

                HttpResponseMessage response = await client.GetAsync($"users/{UserId}");

                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content.ReadAsAsync<Users>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                //var dataList = stringData.Where(x => x.UserId == UserId);

                //return View(dataList.ToList());
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int UserId, Users users)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PutAsJsonAsync($"users/{UserId}", users);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("AdminDashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }

                    /// string stringData = JsonConvert.SerializeObject(obj);
                    // var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    //HttpResponseMessage response = client.PutAsync("/tenant/", contentData).Result;
                    //ViewBag.Message = response.Content.ReadAsStringAsync().Result;

                    //client.BaseAddress = new Uri(BaseUrl);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //HttpResponseMessage response = client.GetAsync("/tenant" + UserId).Result;
                    //string stringData = response.Content.ReadAsStringAsync().Result;


                }
                return RedirectToAction("AdminDashboard");
            }

            return View(users);
        }


        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login");
        }





    }
}
