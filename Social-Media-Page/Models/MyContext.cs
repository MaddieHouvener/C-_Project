using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    // the MyContext class representig a session with our MySQL 
    // database allowing us to query for or save data
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        // the "Monsters" table name will come from the DbSet variable name
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}