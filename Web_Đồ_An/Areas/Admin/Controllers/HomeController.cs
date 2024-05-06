using Microsoft.AspNetCore.Mvc;

namespace Web_Đồ_An.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
