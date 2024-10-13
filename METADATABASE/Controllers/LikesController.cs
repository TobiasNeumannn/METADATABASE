using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Areas.Identity.Data;
using METADATABASE.Models;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using System.Security.Claims;


namespace METADATABASE.Controllers
{
    public class LikesController : Controller
    {
        private readonly METAContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public LikesController(METAContext context, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        // GET: Likes
        public async Task<IActionResult> Index(int? postId, int? commentId, int pageNumber = 1, int pageSize = 5)
        {
            // code that allows the comment/post preview to appear in the likes index efven when they have no likes
            if (postId != null)
            {
                var post = await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.PostsID == postId);
                if (post == null)
                {
                    return NotFound();
                }

                var likesQuery = _context.Likes.Where(l => l.PostsID == postId).Include(l => l.User);
                var totalLikes = await likesQuery.CountAsync();

                // Get the likes with pagination
                var likesList = await likesQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                ViewBag.Post = post;  // Pass the post to the view
                ViewBag.TotalLikes = totalLikes; // Total likes for pagination
                ViewBag.CurrentPage = pageNumber; // Current page number
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalLikes / pageSize); // Total pages

                return View(likesList);

            }
            else if (commentId != null)
            {
                var comment = await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.CommentsID == commentId);
                if (comment == null)
                {
                    return NotFound();
                }

                // Query likes for the comment
                var likesQuery = _context.Likes.Where(l => l.CommentsID == commentId).Include(l => l.User);
                var totalLikes = await likesQuery.CountAsync();

                // Get the likes with pagination
                var likesList = await likesQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                ViewBag.Comment = comment;  // Pass the comment to the view
                ViewBag.TotalLikes = totalLikes; // Total likes for pagination
                ViewBag.CurrentPage = pageNumber; // Current page number
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalLikes / pageSize); // Total pages

                return View(likesList);
            }

            return NotFound();

        }


        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes
                .Include(l => l.Comment)
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikesID == id);
            if (likes == null)
            {
                return NotFound();
            }

            return View(likes);
        }

        // GET: Likes/Create
        public IActionResult Create(int? postId, int? commentId)
        {

            // Pass postId or commentId to the view
            ViewBag.PostId = postId;
            ViewBag.CommentId = commentId;

            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikesID,Id,PostsID,CommentsID")] Likes likes)
        {
            // Get the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID

            // Check if the like is for a post or a comment, and handle the checks accordingly
            if (likes.PostsID != null)
            {
                // If the like is for a post, check if the user already liked this specific post
                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.PostsID == likes.PostsID && l.UserId == userId);

                if (existingLike != null)
                {
                    TempData["ErrorMessage"] = "You have already liked this post.";
                    return RedirectToAction("Create", new { postId = likes.PostsID });
                }
            }
            else if (likes.CommentsID != null)
            {
                // If the like is for a comment, check if the user already liked this specific comment
                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.CommentsID == likes.CommentsID && l.UserId == userId);

                if (existingLike != null)
                {
                    TempData["ErrorMessage"] = "You have already liked this comment.";
                    return RedirectToAction("Create", new { commentId = likes.CommentsID });
                }
            }

            if (ModelState.IsValid)
            {
                likes.UserId = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
                _context.Add(likes);
                await _context.SaveChangesAsync();
                if (likes.PostsID != null)
                {
                    return RedirectToAction("Index", new { postId = likes.PostsID });

                }
                else
                {
                    return RedirectToAction("Index", new { commentId = likes.CommentsID });
                }
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", likes.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", likes.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", likes.UserId);
            return View(likes);
        }

        // cant edit likes

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes
                .Include(l => l.Comment)
                .Include(l => l.Post)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LikesID == id);
            if (likes == null)
            {
                return NotFound();
            }

            return View(likes);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var likes = await _context.Likes.FindAsync(id);
            if (likes != null)
            {
                _context.Likes.Remove(likes);
            }

            await _context.SaveChangesAsync();
            if (likes.PostsID != null)
            {
                return RedirectToAction("Index", new { postId = likes.PostsID });

            }
            else
            {
                return RedirectToAction("Index", new { commentId = likes.CommentsID });
            }
        }

        private bool LikesExists(int id)
        {
            return _context.Likes.Any(e => e.LikesID == id);
        }
    }
}
