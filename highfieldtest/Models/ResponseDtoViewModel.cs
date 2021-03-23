using System.Collections.Generic;

namespace highfieldtest.Models
{
    public class ResponseDtoViewModel
    {
        public IEnumerable<UserViewModel> users;

        public System.Collections.Generic.IList<AgePlusTwentyDtoViewModel> ages;

        public IDictionary<string, int> colours;
    }
}