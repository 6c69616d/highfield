using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using highfieldtest.Models;

namespace highfieldtest.Controllers
{
    public class TopColoursDtoController : Controller
    {
        // GET: TopColoursDto
        public ActionResult Index()
        {
            System.Collections.Generic.IList<UserViewModel> users = null;
            IDictionary<string, int> topColoursDtos = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://recruitment.highfieldqualifications.com/api/");
                //HTTP GET
                var responseTask = client.GetAsync("test");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserViewModel>>();
                    readTask.Wait();

                    users = readTask.Result;

                    foreach (var user in users)
                    {
                        if (user.FavouriteColour != null)
                        {
                            if (topColoursDtos.ContainsKey(user.FavouriteColour))
                            {
                                int value = topColoursDtos[user.FavouriteColour];
                                value += 1;
                                topColoursDtos.Remove(user.FavouriteColour);
                                topColoursDtos.Add(user.FavouriteColour, value);

                            }
                            else
                            {
                                topColoursDtos.Add(user.FavouriteColour, 1);
                            }
                        }
                        
                    }

                    if (topColoursDtos != null)
                    {
                        topColoursDtos = topColoursDtos.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                    }
                }
                else //web api sent error response 
                {

                    users = new List<UserViewModel>();

                    ModelState.AddModelError(string.Empty, "Error retrieving users");
                }
            }
            return View(topColoursDtos);
        }
    }
}