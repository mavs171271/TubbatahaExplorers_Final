using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisualStudio.Areas.Identity.Data;
using VisualStudio.Models;

namespace VisualStudio.Controllers
{
    public class FeedbackDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedbackDatasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FeedbackDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedbackDatas.ToListAsync());
        }

        
        // GET: FeedbackDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Types,Body")] FeedbackData feedbackData)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            // Set the UserId to the currently logged-in user's ID
            feedbackData.UserId = userId;

            // Set the current date and time for DateCreated
            feedbackData.DateCreated = DateTime.Now;

            // Add to the database and save changes
            _context.Add(feedbackData);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }







        private bool FeedbackDataExists(int id)
        {
            return _context.FeedbackDatas.Any(e => e.Rfid == id);
        }
    }
}
