using System.Collections.Generic;

namespace highfieldtest.Models
{
    public class ResponseDtoViewModel
    {
        public IEnumerable<UserViewModel> Users;

        public System.Collections.Generic.IList<AgePlusTwentyDtoViewModel> Ages;

        public IDictionary<string, int> Colours;
    }
}