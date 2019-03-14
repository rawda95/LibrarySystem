using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    public class BorrowMember
    {
        [Key, Column(Order = 0)]
        [ForeignKey("MemberModels")]
        public string MemberId { get; set; }
        [Key, Column(Order = 1)]

        [ForeignKey("book")]
        public int BookId { get; set; }

        public DateTime DateBorrow { get; set; }
        public DateTime returnDate { get; set; }
        public int Duration { get; set; }
        public bool ReturnBook { get; set; }

        public int NumoFBorrow { get; set; }


        public virtual MemberModels MemberModels { get; set; }
        public virtual book book { get; set; }

    }
}
