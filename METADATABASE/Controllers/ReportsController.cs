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
    public class ReportsController : Controller
    {
        private readonly METAContext _context;

        public ReportsController(METAContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var mETAContext = _context.Reports.Include(r => r.Comment).Include(r => r.Post).Include(r => r.User);
            return View(await mETAContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID");
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportsID,Id,PostsID,CommentsID,Content,Creation")] Reports reports)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(reports);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", reports.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", reports.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Email", reports.Id);
            return View(reports);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reports = await _context.Reports.FindAsync(id);
            if (reports == null)
            {
                return NotFound();
            }
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", reports.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", reports.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Email", reports.Id);
            return View(reports);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportsID,Id,PostsID,CommentsID,Content,Creation")] Reports reports)
        {
            if (id != reports.ReportsID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(reports);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportsExists(reports.ReportsID))
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
            ViewData["CommentsID"] = new SelectList(_context.Comments, "CommentsID", "CommentsID", reports.CommentsID);
            ViewData["PostsID"] = new SelectList(_context.Posts, "PostsID", "PostsID", reports.PostsID);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Email", reports.Id);
            return View(reports);
        }

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
            return RedirectToAction(nameof(Index));
        }

        private bool ReportsExists(int id)
        {
            return _context.Reports.Any(e => e.ReportsID == id);
        }
    }
}
