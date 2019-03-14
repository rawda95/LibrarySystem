using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    public class MemberRead
    {


        [Key, Column(Order = 0)]
        [ForeignKey("Member")]
        public string MemberId { get; set; }
        [Key, Column(Order = 1)]

        [ForeignKey("book")]
        public int BookId { get; set; }

        public DateTime DataReading { get; set; }
        public bool ReturnBook { set; get; }
        public int numOfread { get; set; }

        public virtual MemberModels Member { get; set; }
        public virtual book book { get; set; }


    }

}
