using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }

        public byte[] Photo { get; set; }
        public int FlagId { get; set; } = 0;

        public virtual MemberModels MemberModel { set; get; }
        public virtual EmployeeModel EmployeeModel { set; get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    //private object dateTime;

    //    public ApplicationDbContext()
    //        : base("name=library", throwIfV1Schema: false)

    //    {
    //    }

    //    public DbSet<EmployeeModel> Employees { get; set; }
    //    public DbSet<MemberModels> Members { get; set; }



    //    public virtual DbSet<book> Books { get; set; }
    //    public virtual DbSet<Category> Categories { get; set; }
    //    public virtual DbSet<Author> Authors { get; set; }
    //    public virtual DbSet<Publisher> Publishers { get; set; }
    //    public virtual DbSet<flag> Flags { get; set; }
    //    //public virtual DbSet<members> Members { get; set; }
    //    //public virtual DbSet<MemberBorrow> MemberBorrows { get; set; }
    //    public virtual DbSet<BorrowMember> BorrowMembers { get; set; }
    //    public virtual DbSet<MemberRead> ReadMembers { get; set; }








    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);

    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}

}
