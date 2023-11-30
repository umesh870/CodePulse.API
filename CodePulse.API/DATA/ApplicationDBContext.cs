using CodePulse.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.DATA
{
    public class ApplicationDBContext : DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {


        }

        public DbSet<BlogPost> Blogposts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
    }
}