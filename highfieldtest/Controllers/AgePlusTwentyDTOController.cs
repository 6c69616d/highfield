using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using highfield.Models;
using highfieldtest.Models;

namespace highfieldtest.Controllers
{
    public class AgePlusTwentyDTOController : Controller
    {
        // GET: AgePlusTwentyDTO
        public ActionResult Index()
        {
            System.Collections.Generic.IList<UserViewModel> users = null;
            System.Collections.Generic.IList<AgePlusTwentyDtoViewModel> agePlusTwentyDTOs = null;

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
                        AgePlusTwentyDtoViewModel userToAdd = new AgePlusTwentyDtoViewModel();
                        userToAdd.userId = user.Id;
                        userToAdd.originalAge = CalculateAge(user.DateOfBirth);
                        userToAdd.agePlusTwenty = CalculateAgePlusTwenty(userToAdd.originalAge);
                        agePlusTwentyDTOs.Add(userToAdd);
                    }
                }
                else //web api sent error response 
                {
                    
                    users = new List<UserViewModel>();

                    ModelState.AddModelError(string.Empty, "Error retrieving users");
                }
            }
            return View(agePlusTwentyDTOs);

        }

        private int CalculateAge(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
                age = age - 1;

            return age;
        }

        private int CalculateAgePlusTwenty(int originalAge)
        {
           return originalAge + 20;
        }

    }
}