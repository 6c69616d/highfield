using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using highfieldtest.Models;

namespace highfieldtest.Controllers
{
    public class ResponseDtoController : Controller
    {
        // GET: ResponseDto
        public ActionResult Index()
        {
            ResponseDtoViewModel responseDto = new ResponseDtoViewModel();
            responseDto.Users = GetUsers();
            //going to be multiple enumerations so using an array 
            var responseDtoUsers = responseDto.Users as UserViewModel[] ?? responseDto.Users.ToArray();
            responseDto.Colours = GetColours(responseDtoUsers);
            responseDto.Ages = GetAges(responseDtoUsers);
            return View(responseDto);
        }

        private System.Collections.Generic.IList<AgePlusTwentyDtoViewModel> GetAges(IEnumerable<UserViewModel> responseDtoUsers)
        {
            System.Collections.Generic.IList<AgePlusTwentyDtoViewModel> ages = null;
            foreach (var user in responseDtoUsers)
            {
                AgePlusTwentyDtoViewModel userToAdd = new AgePlusTwentyDtoViewModel();
                userToAdd.UserId = user.Id;
                userToAdd.OriginalAge = CalculateAge(user.DateOfBirth);
                userToAdd.AgePlusTwenty = CalculateAgePlusTwenty(userToAdd.OriginalAge);
                ages.Add(userToAdd);
            }

            return ages;
        }

        private IDictionary<string, int> GetColours(IEnumerable<UserViewModel> responseDtoUsers)
        {

            IDictionary<string, int> colours = null;
            foreach (var user in responseDtoUsers)
            {
                if (user.FavouriteColour != null)
                {
                    if (colours.ContainsKey(user.FavouriteColour))
                    {
                        int value = colours[user.FavouriteColour];
                        value += 1;
                        colours.Remove(user.FavouriteColour);
                        colours.Add(user.FavouriteColour, value);

                    }
                    else
                    {
                        colours.Add(user.FavouriteColour, 1);
                    }
                }

            }

            if (colours != null)
            {
                colours = colours.OrderByDescending(x => x.Value).ThenBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            }

            return colours;
        }

        private IEnumerable<UserViewModel> GetUsers()
        {
            IEnumerable<UserViewModel> users = null;

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
                }
                else //web api sent error response 
                {


                    users = Enumerable.Empty<UserViewModel>();

                    ModelState.AddModelError(string.Empty, "Error retrieving users");
                }
            }

            return users;
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