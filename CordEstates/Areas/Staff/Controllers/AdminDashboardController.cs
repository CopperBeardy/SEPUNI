using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {

        public IActionResult Index()
        {
            return View(nameof(Index));

        }
    }
}