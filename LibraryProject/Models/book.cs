using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryProject.Models
{
    [Table("Book")]

    public class book
    {

        public book()
        {
            MemberBorrow = new List<BorrowMember>();
            MemberReads = new List<MemberRead>();

        }

        [Key]
        public int id { get; set; }
        //[Index(IsUnique = true)]

        public string name { get; set; }


        public DateTime PublisherDate { get; set; }
        public string Edition { get; set; }
        public int Pages { get; set; }

        public int Copies { get; set; }

        public int NumOfbrrow { get; set; }

        public int NumOfread { get; set; }
        public bool Aviable { get; set; }

        public int ShielfNumber { get; set; }

        public DateTime dateArived { get; set; }


        [ForeignKey("Category")]
        public int category_id { get; set; }

        [ForeignKey("Author")]
        public int author_id { get; set; }

        [ForeignKey("Publisher")]
        public int publisher_id { get; set; }


        public virtual Category Category { set; get; }

        public virtual Author Author { set; get; }
        public virtual Publisher Publisher { set; get; }


        public virtual List<BorrowMember> MemberBorrow { get; set; }
        public virtual List<MemberRead> MemberReads { get; set; }




    }
}
