using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.Models
{
    public class Author
    {
        public Author()
        {
            books = new List<book>();
        }
        [Key]
        public int id { get; set; }
        // [Index(IsUnique = true)]

        public string name { get; set; }

        public virtual List<book> books { get; set; }

    }
}
