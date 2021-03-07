using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{

    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }

        /*
        * DbSet represents objects that will be mapped from the database.
        */
        public DbSet<AppUser> Users {get; set;}
    }
}