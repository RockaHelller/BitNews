using BitNews.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitNews.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<Tag>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<News>().HasQueryFilter(m => !m.SoftDelete);
            modelBuilder.Entity<NewsImage>().HasQueryFilter(m => !m.SoftDelete);



        }






    }
}
