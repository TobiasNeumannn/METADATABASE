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
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                                     .Include(p => p.User)
                                     .Include(p => p.Comments)  // Include the Comments navigation property
                                     .Include(p => p.Likes)  // Include the Likes navigation property
                                     .ToListAsync();

            foreach (var post in posts)
            {
                post.CommentsCount = post.Comments.Count;  // Set the CommentsCount property
                post.LikesCount = post.Likes.Count;
            }

            return View(posts);
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostsID,Description,Creation,Title,Locked,Id")] Posts posts)
        {
            if (!ModelState.IsValid)
            {
                posts.Id = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
                posts.Creation = DateTime.Now; // Set the current time
                _context.Add(posts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", posts.Id);
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", posts.Id);
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostsID,Description,Creation,Title,Locked,Id")] Posts posts)
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", posts.Id);
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
            var posts = await _context.Posts.FindAsync(id);
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
