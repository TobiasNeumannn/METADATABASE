using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Areas.Identity.Data;
using METADATABASE.Models;

namespace METADATABASE.Controllers
{
    public class LikesController : Controller
    {
        private readonly METAContext _context;

        public LikesController(METAContext context)
        {
            _context = context;
        }

        // GET: Likes
        public async Task<IActionResult> Index(int? postId)
        {
            if (postId == null)
            {
                return NotFound();
            }

            var likes = _context.Likes.Where(c => c.PostsID == postId).Include(c => c.Post).Include(c => c.User);
            return View(await likes.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID");
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikesID,UserID,Pfp,PostsID,CommentsID")] Likes likes)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(likes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", likes.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", likes.PostsID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email", likes.UserID);
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email", likes.UserID);
            return View(likes);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikesID,UserID,Pfp,PostsID,CommentsID")] Likes likes)
        {
            if (id != likes.LikesID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email", likes.UserID);
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
