using Web_Đồ_An.Models;
using Web_Đồ_An.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Web_Đồ_An.Controllers
{
	public class ShopCartController : Controller
	{
		private readonly AppDbContext _db;

		private List<ShopCart> carts = new List<ShopCart>();
		public ShopCartController(AppDbContext db)
		{
			_db = db;
		}
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var cartInSession = HttpContext.Session.GetString("My-Cart");
			if (cartInSession != null)
			{
				// nếu cartInSession không null thì gán dữ liệu cho biến carts
				// Chuyển san dữ liệu json
				carts = JsonConvert.DeserializeObject<List<ShopCart>>(cartInSession);
			}
			base.OnActionExecuting(context);
		}
		public IActionResult Index()
		{
			decimal total = 0;
			var count = 0;
			foreach (var item in carts)
			{
				total += item.Quantity * item.PriceSale;
				count++;
			}
			ViewBag.Total = total;
			ViewBag.Count = count;
			return View(carts);
		}
/*        [Route("ShopCart/Add/{id}")]
*/        public IActionResult Add(int id )
		{
			if (carts.Any(c => c.ProductId == id))
			{
				carts.Where(c => c.ProductId == id).First().Quantity += 1;
			}
			else
			{
				var p = _db.Products.FirstOrDefault(x => x.ProductId == id);

				var item = new ShopCart()
				{
					ProductId = p.ProductId,
					Title = p.Title,
					Price = (decimal)p.Price,
					PriceSale = (decimal)p.PriceSale,
					Quantity = 1,
					Image = p.Image,
					Total = (decimal)p.PriceSale * 1
				};
				carts.Add(item);
			}
			HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
			return RedirectToAction("Index");
		}

		public IActionResult Remove(int id)
		{
			if (carts.Any(c => c.ProductId == id))
			{
				var item = carts.Where(c => c.ProductId == id).First();
				carts.Remove(item);
				HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
			}
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id, int quantity)
		{
			if (carts.Any(c => c.ProductId == id))
			{
				carts.Where(c => c.ProductId == id).First().Quantity = quantity;
				HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
			}
			return RedirectToAction("Index");
		}
		public IActionResult Clear()
		{
			HttpContext.Session.Remove("My-Cart");
			return RedirectToAction("Index");
		}
	}
}