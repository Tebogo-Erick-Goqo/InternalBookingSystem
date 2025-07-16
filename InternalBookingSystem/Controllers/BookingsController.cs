using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InternalBookingSystem.Data;
using InternalBookingSystem.Models;

namespace InternalBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Resource);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Resource)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose,IsCancelled")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool hasConflict = _context.Bookings.Any(b =>
                        b.ResourceId == booking.ResourceId &&
                        b.Id != booking.Id &&
                        booking.StartTime < b.EndTime &&
                        booking.EndTime > b.StartTime
                    );

                    if (hasConflict)
                    {
                        ModelState.AddModelError("", "This resource is already booked during the requested time. Please choose another slot or resource.");
                    }
                    else
                    {
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (optional: use logging framework like Serilog or built-in ILogger)
                    Console.WriteLine($"Error creating booking: {ex.Message}");

                    // Show user-friendly message
                    ModelState.AddModelError("", "An unexpected error occurred while saving the booking. Please try again later.");
                }
            }

            // Always repopulate dropdown
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Name", booking.ResourceId);
            return View(booking);
        }

    }
}

        

