using Microsoft.AspNetCore.Mvc;

namespace Web_Đồ_An.Controllers
{
	public class ProductCategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
