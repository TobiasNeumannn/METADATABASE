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
using System.Security.Claims;


namespace METADATABASE.Controllers
{
    public class ReportsController : Controller
    {
        private readonly METAContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public ReportsController(METAContext context, UserManager<Users> userManager, SignInManager<Users> signInManager)
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

        // GET: Reports
        public async Task<IActionResult> Index(int? postId, int? commentId, string sortOrder, string searchString, int pageNumber = 1, int pageSize = 5)
        {
            ViewBag.CurrentSort = sortOrder; // Keep track of the current sort order
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "Date";
            ViewBag.UserSortParm = sortOrder == "User" ? "user_desc" : "User";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

            IQueryable<Reports> reportsQuery;

            // Code to allow the comment/post preview to appear in the reports index even when they have no reports
            if (postId != null)
            {
                var post = await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.PostsID == postId);
                if (post == null)
                {
                    return NotFound();
                }

                reportsQuery = _context.Reports.Where(l => l.PostsID == postId).Include(l => l.User);
                ViewBag.Post = post;  // Pass the post to the view
            }
            else if (commentId != null)
            {
                var comment = await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.CommentsID == commentId);
                if (comment == null)
                {
                    return NotFound();
                }

                reportsQuery = _context.Reports.Where(l => l.CommentsID == commentId).Include(l => l.User);
                ViewBag.Comment = comment;  // Pass the comment to the view
            }
            else
            {
                // Initialize reportsQuery to include all reports if neither postId nor commentId are provided
                reportsQuery = _context.Reports.Include(r => r.User);
            }

            // Filtering logic
            if (!String.IsNullOrEmpty(searchString))
            {
                reportsQuery = reportsQuery.Where(r => r.Content.Contains(searchString) || r.User.UserName.Contains(searchString));
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "date_desc":
                    reportsQuery = reportsQuery.OrderByDescending(r => r.Creation);
                    break;
                case "Date":
                    reportsQuery = reportsQuery.OrderBy(r => r.Creation);
                    break;
                case "user_desc":
                    reportsQuery = reportsQuery.OrderByDescending(r => r.User.UserName);
                    break;
                case "User":
                    reportsQuery = reportsQuery.OrderBy(r => r.User.UserName);
                    break;
                default:
                    reportsQuery = reportsQuery.OrderBy(r => r.Creation);
                    break;
            }

            var totalReports = await reportsQuery.CountAsync();
            var reportsList = await reportsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalReports = totalReports; // Total reports for pagination
            ViewBag.CurrentPage = pageNumber; // Current page number
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalReports / pageSize); // Total pages

            return View(reportsList);
        }


        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reports = await _context.Reports
                .Include(r => r.Comment)
                .Include(r => r.Post)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportsID == id);
            if (reports == null)
            {
                return NotFound();
            }

            return View(reports);
        }

        // GET: Reports/Create
        public IActionResult Create(int? postId, int? commentId)
        {
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID");
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Email");

            // Pass postId or commentId to the view
            ViewBag.PostId = postId;
            ViewBag.CommentId = commentId;

            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportsID,Id,PostsID,CommentsID,Content,Creation")] Reports reports)
        {
            // Get the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID

            // Check if the report is for a post or a comment, and handle the checks accordingly
            if (reports.PostsID != null)
            {
                // If the report is for a post, check if the user already reported this specific post
                var existingReport = await _context.Reports
                    .FirstOrDefaultAsync(l => l.PostsID == reports.PostsID && l.UserId == userId);

                if (existingReport != null)
                {
                    TempData["ErrorMessage"] = "You have already reported this post.";
                    return RedirectToAction("Create", new { postId = reports.PostsID });
                }
            }
            else if (reports.CommentsID != null)
            {
                // If the report is for a comment, check if the user already reported this specific comment
                var existingReport = await _context.Reports
                    .FirstOrDefaultAsync(l => l.CommentsID == reports.CommentsID && l.UserId == userId);

                if (existingReport != null)
                {
                    TempData["ErrorMessage"] = "You have already reported this comment.";
                    return RedirectToAction("Create", new { commentId = reports.CommentsID });
                }
            }

            reports.Creation = DateTime.Now; // Set the current time

            if (ModelState.IsValid)
            {
                reports.UserId = await GetCurrentUserIdAsync(); // Set the UserId to the currently signed-in user's ID
                _context.Add(reports);
                await _context.SaveChangesAsync();
                if (reports.PostsID != null)
                {
                    return RedirectToAction("Index", new { postId = reports.PostsID });

                }
                else
                {
                    return RedirectToAction("Index", new { commentId = reports.CommentsID });
                }
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", reports.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", reports.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", reports.UserId);
            return View(reports);
        }

        // cant edit REPORTS

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reports = await _context.Reports
                .Include(r => r.Comment)
                .Include(r => r.Post)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportsID == id);
            if (reports == null)
            {
                return NotFound();
            }

            return View(reports);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reports = await _context.Reports.FindAsync(id);
            if (reports != null)
            {
                _context.Reports.Remove(reports);
            }

            await _context.SaveChangesAsync();
            if (reports.PostsID != null)
            {
                return RedirectToAction("Index", new { postId = reports.PostsID });

            }
            else
            {
                return RedirectToAction("Index", new { commentId = reports.CommentsID });
            }
        }

        private bool ReportsExists(int id)
        {
            return _context.Reports.Any(e => e.ReportsID == id);
        }
    }
}
