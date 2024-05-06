using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Đồ_An.Models;
using Web_Đồ_An.Models.Entities;

namespace Web_Đồ_An.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

		public IActionResult Index()
		{
			var products = _db.Products/*.Include("Products")*/.ToList();
			// Gọi view component và trả về kết quả
			return View (products);
		}


		public ActionResult Detail(string alias, int id)
		{

            var products = _db.Products.Include(p => p.ProductCategory).ToList();
            
            var item = _db.Products.Find(id) ;
            if (item != null)
			{
				_db.Products.Attach(item);
				item.ViewCount = item.ViewCount + 1;
				_db.Entry(item).Property(x => x.ViewCount).IsModified = true;
				_db.SaveChanges();
			}

			return View(item);
		}



        public ActionResult ProductCategory(string alias, int id)
        {
            var items = _db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

    }
}
