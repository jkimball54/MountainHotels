using Microsoft.AspNetCore.Mvc;
using MountainHotels.DataAccess;

namespace MountainHotels.Controllers
{
    public class HotelsController : Controller
    {
        private readonly MountainHotelsContext _context;

        public HotelsController(MountainHotelsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hotels = _context.Hotels;
            return View(hotels);
        }
    }
}
