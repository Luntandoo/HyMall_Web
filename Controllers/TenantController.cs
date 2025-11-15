using System;
using System.Linq;
using System.Threading.Tasks;
using HyMall_App.Data;
using HyMall_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HyMall_App.Controllers
{
    [Authorize(Roles = "Tenant")]
    public class TenantController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TenantController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ------------------------------
        // DASHBOARD
        // ------------------------------
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserName = user?.Name ?? "Tenant";
            return View();
        }

        // ------------------------------
        // INVENTORY CHECK FORM + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult InventoryCheck()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InventoryCheck(InventoryCheck model)
        {
            if (ModelState.IsValid)
            {
                _context.InventoryChecks.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "✅ Inventory check recorded successfully!";
                return RedirectToAction("ViewInventoryCheck");
            }

            TempData["Error"] = "⚠️ Please correct the errors and try again.";
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewInventoryCheck()
        {
            var records = _context.InventoryChecks
                .OrderByDescending(i => i.ProductName)
                .ToList();
            return View(records);
        }

        // ------------------------------
        // ANNOUNCEMENTS FORM + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult Announcement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Announcement(Announcement model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                model.Status = "Pending";
                _context.Announcements.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "📢 Announcement sent to admin for review.";
                return RedirectToAction("ViewAnnouncements");
            }

            TempData["Error"] = "⚠️ Please fix the errors before submitting.";
            return View(model);
        }

        public IActionResult ViewAnnouncements()
        {
            var announcements = _context.Announcements
                .OrderByDescending(a => a.Status)
                .ToList();
            return View(announcements);
        }

        // ------------------------------
        // CUSTOMER FEEDBACK FORM + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult CustomerFeedback()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerFeedback(CustomerFeedback model)
        {
            if (ModelState.IsValid)
            {
                // Set CreatedAt if not already set in model
                if (model.CreatedAt == default)
                {
                    model.CreatedAt = DateTime.Now;
                }

                _context.CustomerFeedbacks.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "💬 Feedback recorded successfully!";
                return RedirectToAction("ViewFeedback");
            }

            TempData["Error"] = "⚠️ Please fix the errors before submitting.";
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewFeedback()
        {
            var feedbacks = _context.CustomerFeedbacks
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
            return View(feedbacks);
        }

        [HttpPost]
        public IActionResult UpdateResponseStatus(int id, string responseStatus)
        {
            try
            {
                // Find the feedback record
                var feedback = _context.CustomerFeedbacks.Find(id);
                if (feedback != null)
                {
                    // Update the response status
                    feedback.ResponseStatus = responseStatus;

                    // Optionally update a modified date if you have that field
                    // feedback.UpdatedAt = DateTime.Now;

                    // Save changes to database
                    _context.SaveChanges();

                    // Success message
                    TempData["Success"] = "Response status updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Feedback record not found!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating response status: " + ex.Message;
            }

            // Redirect back to the customer feedback list
            return RedirectToAction("ViewFeedback");
        }


        // ------------------------------
        // PROMOTIONS FORM + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult Promotion()
        {
            return View(new Promotion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Promotion(Promotion model)
        {
            if (ModelState.IsValid)
            {
                _context.Promotions.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "🎉 Promotion submitted successfully!";
                return RedirectToAction("PromotionList");
            }

            TempData["Error"] = "⚠️ Please fix the errors before submitting.";
            return View(model);
        }

        public IActionResult PromotionList()
        {
            var promotions = _context.Promotions
                .OrderByDescending(p => p.StartDate)
                .ToList();
            return View(promotions);
        }

        // ------------------------------
        // PRODUCT SHORTAGE FORM + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult ProductShortageReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductShortageReport(ProductShortageReport model)
        {
            if (ModelState.IsValid)
            {
                _context.ProductShortageReport.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "📦 Shortage reported successfully!";
                return RedirectToAction("ViewShortages");
            }

            TempData["Error"] = "⚠️ Please correct the form errors and resubmit.";
            return View(model);
        }

        public IActionResult ViewShortages()
        {
            var shortages = _context.ProductShortageReport
                .OrderByDescending(s => s.ReportedOn)
                .ToList();
            return View(shortages);
        }

        // ------------------------------
        // SHOP DETAILS FORM
        // ------------------------------
        [HttpGet]
        public IActionResult ShopDetail()
        {
            var model = _context.ShopDetail.FirstOrDefault() ?? new ShopDetail();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShopDetail(ShopDetail model, string[] DaysOpen)
        {
            if (ModelState.IsValid)
            {
                // Combine multiple selected days into a single comma-separated string
                if (DaysOpen != null && DaysOpen.Length > 0)
                {
                    model.DaysOpen = string.Join(",", DaysOpen);
                }

                // Check if a record with the same shop name exists
                var existing = _context.ShopDetail.FirstOrDefault(s => s.ShopName == model.ShopName);
                if (existing != null)
                {
                    existing.Directions = model.Directions;
                    existing.OpeningTime = model.OpeningTime;
                    existing.ClosingTime = model.ClosingTime;
                    existing.DaysOpen = model.DaysOpen;
                    existing.ContactNumber = model.ContactNumber;
                    existing.Email = model.Email;
                }
                else
                {
                    _context.ShopDetail.Add(model);
                }

                _context.SaveChanges();

                // Notify and redirect tenant back to dashboard
                TempData["Success"] = "🏪 Shop details saved successfully!";
                return RedirectToAction("Dashboard", "Tenant");
            }

            TempData["Error"] = "⚠️ Please fix the errors before submitting.";
            return View(model);
        }




        // ------------------------------
        // EVENT SUBMISSION + VIEW
        // ------------------------------
        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(Event model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                model.SubmittedBy = user?.Email ?? "Unknown";
                model.Status = "Pending";
                model.CreatedAt = DateTime.UtcNow;
                _context.Events.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "🎊 Event submitted for admin approval.";
                return RedirectToAction("MyEvents");
            }

            TempData["Error"] = "⚠️ Please correct the errors before submitting.";
            return View(model);
        }

        public async Task<IActionResult> MyEvents()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email ?? string.Empty;
            var events = _context.Events
                .Where(e => e.SubmittedBy == email)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
            return View(events);
        }
    }
}
