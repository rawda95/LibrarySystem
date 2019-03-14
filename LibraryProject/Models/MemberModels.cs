using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    [Table("Members")]
    public class MemberModels
    {

        public MemberModels()
        {

            BorrowBooks = new List<BorrowMember>();
            ReadBooks = new List<MemberRead>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        [ForeignKey("User")]
        public string MemeberId { get; set; }
        public bool CanBorrow { get; set; } = true;
        public DateTime DatedExceed { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("status")]
        public int status_id { get; set; }


        public virtual List<BorrowMember> BorrowBooks { get; set; }
        public virtual List<MemberRead> ReadBooks { get; set; }

        public virtual List<flag> status { get; set; }

    }
}
