using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
    public class Publisher
    {
        public Publisher()
        {
            books = new List<book>();
        }

        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public virtual List<book> books { get; set; }

    }
}
