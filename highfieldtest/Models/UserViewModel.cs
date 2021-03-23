using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using highfieldtest.Models;

namespace highfieldtest.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FavouriteColour { get; set; }

        public AgePlusTwentyDtoViewModel AgePlus { get; set; }
    }
}