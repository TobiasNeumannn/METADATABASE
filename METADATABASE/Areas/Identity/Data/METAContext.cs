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

                string flagFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "seeded.flag");

                if (File.Exists(flagFilePath)) // check if the seeding flag file exists - exists to ensure seeding only happens once, when needed
                {
                    // Seeding has already been done; exit
                    return;
                }

                var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
                var normalUser = await userManager.FindByEmailAsync("user@user.com");

                    // Reset the PK for all tables
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Posts', RESEED, 1)");
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Comments', RESEED, 1)");
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Likes', RESEED, 1)");
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Reports', RESEED, 1)");

                if (adminUser != null && normalUser != null) {
                    if (!context.Posts.Any())
                    {

                        context.Posts.AddRange(
                            new Posts { Title = "Admin's First Post", Description = "Description for Admin's first post", Creation = DateTime.Now, Locked = false, UserId = adminUser.Id },
                        new Posts { Title = "Admin's Second Post", Description = "Description for Admin's second post", Creation = DateTime.Now, Locked = false, UserId = adminUser.Id },
                        new Posts { Title = "User's First Post", Description = "Description for User's first post", Creation = DateTime.Now, Locked = false, UserId = normalUser.Id }
        );
                        await context.SaveChangesAsync();
                    }

                    // save postIds as variables (avoids the annoying SQL Identity memory issue)
                    var post1 = context.Posts.FirstOrDefault(p => p.Title == "Admin's First Post");
                    var post2 = context.Posts.FirstOrDefault(p => p.Title == "Admin's Second Post");
                    var post3 = context.Posts.FirstOrDefault(p => p.Title == "User's First Post");

                    if (post1 != null && post2 != null && post3 != null) // ensure no postIDs are null in records
                    {

                        if (!context.Comments.Any())
                        {
                            context.Comments.AddRange(
        new Comments { PostsID = post1.PostsID, Content = "Great post!", Creation = DateTime.Now, UserId = adminUser.Id, Correct = false },
        new Comments { PostsID = post2.PostsID, Content = "Thanks for sharing!", Creation = DateTime.Now, UserId = normalUser.Id, Correct = true },
        new Comments { PostsID = post3.PostsID, Content = "Very informative.", Creation = DateTime.Now, UserId = adminUser.Id, Correct = false }
        );
                            await context.SaveChangesAsync();
                        }

                        var comment1 = context.Comments.FirstOrDefault(c => c.PostsID == post1.PostsID);

                        if (comment1 != null) // ensure no commentIDs are null
                        {

                            if (!context.Likes.Any())
                            {
                                context.Likes.AddRange(
                    new Likes { UserId = adminUser.Id, PostsID = post1.PostsID },
                    new Likes { UserId = normalUser.Id, PostsID = post2.PostsID },
                    new Likes { UserId = adminUser.Id, PostsID = comment1.CommentsID }
                );
                                await context.SaveChangesAsync();
                            }

                            if (!context.Reports.Any())
                            {
                                context.Reports.AddRange(
                new Reports { UserId = normalUser.Id, PostsID = post1.PostsID, Content = "Inappropriate content", Creation = DateTime.Now },
                new Reports { UserId = adminUser.Id, PostsID = post2.PostsID, Content = "Spam", Creation = DateTime.Now },
                new Reports { UserId = normalUser.Id, CommentsID = comment1.CommentsID, Content = "Offensive language", Creation = DateTime.Now }
            );
                            }
                        }
                    }
                }

                // Create the flag file to indicate seeding is complete
                Directory.CreateDirectory(Path.GetDirectoryName(flagFilePath));
                File.Create(flagFilePath).Dispose();

                await context.SaveChangesAsync();
            }
        }
    }
}