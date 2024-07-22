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
        public async Task<IActionResult> Index(int? postId, int? commentId)
        {
            if (postId != null)
            {
                var postLikes = _context.Likes.Where(l => l.PostsID == postId).Include(l => l.Post).Include(l => l.User);
                ViewBag.PostId = postId;
                return View(await postLikes.ToListAsync());
            }
            else if (commentId != null)
            {
                var commentLikes = _context.Likes.Where(l => l.CommentsID == commentId).Include(l => l.Comment).Include(l => l.User);
                return View(await commentLikes.ToListAsync());
            }
            else
            {
                return NotFound();
            }
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
            if (!ModelState.IsValid)
            {
                likes.Id = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
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
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", likes.Id);
            return View(likes);
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes.FindAsync(id);
            if (likes == null)
            {
                return NotFound();
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", likes.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", likes.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", likes.Id);
            return View(likes);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikesID,Id,PostsID,CommentsID")] Likes likes)
        {
            if (id != likes.LikesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(likes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikesExists(likes.LikesID))
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
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", likes.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", likes.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", likes.Id);
            return View(likes);
        }

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
            return RedirectToAction(nameof(Index));
        }

        private bool LikesExists(int id)
        {
            return _context.Likes.Any(e => e.LikesID == id);
        }
    }
}
