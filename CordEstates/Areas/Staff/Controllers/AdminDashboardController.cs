using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles ="Admin")]
    public class AdminDashboardController : Controller
    {

        public IActionResult Index()
        {
          return View(nameof(Index));    

        }
    }
}