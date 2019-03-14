using LibraryProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace LibraryProject.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //private object dateTime;

        public ApplicationDbContext()
            : base("name=library", throwIfV1Schema: false)

        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<MemberModels> Members { get; set; }



        public virtual DbSet<book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<flag> Flags { get; set; }
        //public virtual DbSet<members> Members { get; set; }
        //public virtual DbSet<MemberBorrow> MemberBorrows { get; set; }
        public virtual DbSet<BorrowMember> BorrowMembers { get; set; }
        public virtual DbSet<MemberRead> ReadMembers { get; set; }








        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //}

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
    }
}
