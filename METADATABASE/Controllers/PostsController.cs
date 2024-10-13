using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Areas.Identity.Data;
using METADATABASE.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;


namespace METADATABASE.Controllers
{
    public class PostsController : Controller
    {
        private readonly METAContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public PostsController(METAContext context, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // set userID to current signed-in user
        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            // ViewBag variables for sorting column links in the view
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LikesSortParm = sortOrder == "Likes" ? "likes_desc" : "Likes";

            // Query to get posts, with includes for related entities
            IQueryable<Posts> posts = _context.Posts
                                              .Include(p => p.User)
                                              .Include(p => p.Comments)
                                              .Include(p => p.Likes)
                                              .Include(p => p.Reports);

            // Filtering logic
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(p => p.Title.Contains(searchString) || p.User.UserName.Contains(searchString));
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "title_desc":
                    posts = posts.OrderByDescending(p => p.Title);
                    break;
                case "Date":
                    posts = posts.OrderBy(p => p.Creation);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(p => p.Creation);
                    break;
                case "Likes":
                    posts = posts.OrderBy(p => p.Likes.Count);
                    break;
                case "likes_desc":
                    posts = posts.OrderByDescending(p => p.Likes.Count);
                    break;
                default:
                    posts = posts.OrderBy(p => p.Title);
                    break;
            }

            var postList = await posts.ToListAsync();

            // Set the counts for each post
            foreach (var post in postList)
            {
                post.CommentsCount = post.Comments.Count;
                post.LikesCount = post.Likes.Count;
                post.ReportsCount = post.Reports.Count;
            }

            return View(postList);
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostsID == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            //ViewData["CommentsID"] = new SelectList(_context.Posts, "CommentsID", "Title");
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "Title");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostsID,Description,Creation,Title,Locked,UserId")] Posts posts)
        {
            posts.UserId = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
            posts.Creation = DateTime.Now; // Set the current time

            if (ModelState.IsValid)
            {
                _context.Add(posts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", posts.UserId);
            return View(posts);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts.FindAsync(id);
            if (posts == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", posts.UserId);
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostsID,Description,UserId,Title,Locked,Creation")] Posts posts)
        {
            if (id != posts.PostsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.PostsID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserName", posts.UserId);
            return View(posts);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PostsID == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posts = await _context.Posts
                             .Include(p => p.Comments)  // eagerly include children tables, else they'll be null
                                    .ThenInclude(c => c.Likes)   // Include likes of comments
                             .Include(p => p.Comments)
                                    .ThenInclude(c => c.Reports) // Include reports of comments
                             .Include(p => p.Likes)
                             .Include(p => p.Reports)
                             .FirstOrDefaultAsync(p => p.PostsID == id);

            // manual cascade: delete children records

            if (posts.Comments != null) // if post has comments
            {
                foreach (var comment in posts.Comments)
                {
                    // First delete likes and reports of each comment
                    if (comment.Likes != null)
                    {
                        _context.Likes.RemoveRange(comment.Likes);
                    }
                    if (comment.Reports != null)
                    {
                        _context.Reports.RemoveRange(comment.Reports);
                    }
                }
                _context.Comments.RemoveRange(posts.Comments);
            }
            if (posts.Likes != null)
            {
                _context.Likes.RemoveRange(posts.Likes);
            }
            if (posts.Reports != null)
            {
                _context.Reports.RemoveRange(posts.Reports);
            }


            if (posts != null)
            {
                _context.Posts.Remove(posts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.PostsID == id);
        }

        public async Task<IActionResult> Comments(int postId)
        {
            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.PostsID == postId);
            if (post == null)
            {
                return NotFound();
            }

            return View(post.Comments);
        }
    }
}
