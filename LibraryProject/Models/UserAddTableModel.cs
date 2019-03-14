using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class UserAddTableModel
    {
        public RegisterViewModel RegisterViewModel { set; get; }
        public List<ApplicationUser> ApplicationUsers { set; get; }
        public UpdateViewModel UpdateViewModel { get; set; }
    }
}