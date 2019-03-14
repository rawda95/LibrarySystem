using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    [Table("Category")]

    public class Category
    {
        public Category()
        {
            books = new List<book>();
        }
        [Key]
        public int id { get; set; }

        //   [Index(IsUnique = true)]

        public string name { get; set; }

        public virtual List<book> books { get; set; }

    }
}
