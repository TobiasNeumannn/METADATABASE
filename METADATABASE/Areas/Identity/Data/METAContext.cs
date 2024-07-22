using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Models;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace METADATABASE.Areas.Identity.Data
{
    public class METAContext : IdentityDbContext<Users> // Users?
    {
        public METAContext(DbContextOptions<METAContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // DATA SEEDING!
        {
            base.OnModelCreating(modelBuilder);
        }

public static class SeedData
        {
            public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Users> userManager)
            {

                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    Users user = new Users
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        EmailConfirmed = true,
                        PfpName = "admin_pfp.png",
                        ProjName = "Admin Project",
                        ProjDesc = "This is the Admin's project.",
                        ThumbName = "admin_thumb.png"
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");
                }

                if (await userManager.FindByEmailAsync("user@user.com") == null)
                {
                    Users user = new Users
                    {
                        UserName = "user@user.com",
                        Email = "user@user.com",
                        EmailConfirmed = true,
                        PfpName = "user_pfp.png",
                        ProjName = "User Project",
                        ProjDesc = "This is the User's project.",
                        ThumbName = "user_thumb.png"
                    };

                    var result = await userManager.CreateAsync(user, "User@123");
                }

                var context = serviceProvider.GetRequiredService<METAContext>();

                var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
                var normalUser = await userManager.FindByEmailAsync("user@user.com");

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Posts { Title = "Admin's First Post", Description = "Description for Admin's first post", Creation = DateTime.Now, Locked = false, Id = adminUser.Id },
                    new Posts { Title = "Admin's Second Post", Description = "Description for Admin's second post", Creation = DateTime.Now, Locked = false, Id = adminUser.Id },
                    new Posts { Title = "User's First Post", Description = "Description for User's first post", Creation = DateTime.Now, Locked = false, Id = normalUser.Id },
                    new Posts { Title = "User's Second Post", Description = "Description for User's second post", Creation = DateTime.Now, Locked = true, Id = normalUser.Id },
                    new Posts { Title = "Admin's Third Post", Description = "Description for Admin's third post", Creation = DateTime.Now, Locked = false, Id = adminUser.Id }
);
                    context.Comments.AddRange(
new Comments { PostsID = 1, Content = "Great post!", Creation = DateTime.Now, Id = adminUser.Id, Correct = false },
new Comments { PostsID = 2, Content = "Thanks for sharing!", Creation = DateTime.Now, Id = normalUser.Id, Correct = true },
new Comments { PostsID = 3, Content = "Very informative.", Creation = DateTime.Now, Id = adminUser.Id, Correct = false },
new Comments { PostsID = 4, Content = "I learned a lot.", Creation = DateTime.Now, Id = normalUser.Id, Correct = false },
new Comments { PostsID = 5, Content = "Interesting perspective.", Creation = DateTime.Now, Id = adminUser.Id, Correct = true }
);
                    context.Likes.AddRange(
    new Likes { Id = adminUser.Id, PostsID = 1 },
    new Likes { Id = normalUser.Id, PostsID = 2 },
    new Likes { Id = adminUser.Id, PostsID = 3 },
    new Likes { Id = normalUser.Id, PostsID = 4 },
    new Likes { Id = adminUser.Id, CommentsID = 5 }
);
                    context.Reports.AddRange(
    new Reports { Id = normalUser.Id, PostsID = 1, Content = "Inappropriate content", Creation = DateTime.Now },
    new Reports { Id = adminUser.Id, PostsID = 2, Content = "Spam", Creation = DateTime.Now },
    new Reports { Id = normalUser.Id, CommentsID = 3, Content = "Offensive language", Creation = DateTime.Now },
    new Reports { Id = adminUser.Id, CommentsID = 4, Content = "Harassment", Creation = DateTime.Now },
    new Reports { Id = normalUser.Id, PostsID = 5, Content = "False information", Creation = DateTime.Now }
);

                }

                //seed more data here

                await context.SaveChangesAsync();
            }
        }
    }
}