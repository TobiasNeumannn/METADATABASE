using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Areas.Identity.Data;
using METADATABASE.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace METADATABASE.Controllers
{
    public class CommentsController : Controller
    {
        private readonly METAContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public CommentsController(METAContext context, UserManager<Users> userManager, SignInManager<Users> signInManager)
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

        // GET: Comments
        public async Task<IActionResult> Index(int? postId)
        {
            if (postId == null)
            {
                return NotFound();
            }
            ViewBag.PId = postId;
            // navigation + assign postsID to postID
            var comments = _context.Comments
                .Where(c => c.PostsID == postId)
                .Include(c => c.Post)
                            .ThenInclude(p => p.User) // Load the User related to Post
            .Include(c => c.User)
        .Include(p => p.Likes); 


            foreach (var comment in comments)
            {
                comment.LikesCount = comment.Likes.Count;
            }

            return View(await comments.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentsID == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // GET: Comments/Create
        public IActionResult Create(int? PId)
        {
            ViewBag.PId = PId;
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentsID,PostsID,Content,Creation,UserId,Correct")] Comments comments)
        {
            if (!ModelState.IsValid)
            {
                comments.UserId = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
                comments.Creation = DateTime.Now; // Set the current time
                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { postId = comments.PostsID });
            }

            return View(comments);
        }


        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "Title", comments.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", comments.UserId);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentsID,PostsID,Content,Creation,UserId,Correct")] Comments comments)
        {
            if (id != comments.CommentsID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(comments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsExists(comments.CommentsID))
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
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "Title", comments.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", comments.UserId);

            return View(comments);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentsID == id);
            if (comments == null)
            {
                return NotFound();
            }

            //return View(comments);
            return RedirectToAction("Index", new { postId = comments.PostsID });

        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments != null)
            {
                _context.Comments.Remove(comments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsExists(int id)
        {
            return _context.Comments.Any(e => e.CommentsID == id);
        }
    }
}
