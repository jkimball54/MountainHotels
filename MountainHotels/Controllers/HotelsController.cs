using Microsoft.AspNetCore.Mvc;
using MountainHotels.DataAccess;
using MountainHotels.Models;

namespace MountainHotels.Controllers
{
    public class HotelsController : Controller
    {
        private readonly MountainHotelsContext _context;

        public HotelsController(MountainHotelsContext context)
        {
            _context = context;
        }

        // GET: /Hotels
        public IActionResult Index()
        {
            var hotels = _context.Hotels;
            return View(hotels);
        }

        // GET: /Hotels/New
        public IActionResult New()
        {
            return View();
        }

        // POST: /Hotels
        [HttpPost]
        public IActionResult Index(Hotel hotel)
        {
            //Take the movie sent in the request and save it to the database
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            // Redirect back to the index page with all hotels
            return RedirectToAction("Index");
        }
    }
}
