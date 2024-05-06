using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web_Đồ_An.Models;
using Web_Đồ_An.Models.Entities;
using X.PagedList;

namespace Web_Đồ_An.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _db;

        public object DataLocal { get; private set; }

        public PostsController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index( int page=1)
        {

            int limit = 6;
            var items = _db.Posts.ToPagedList(page, limit);
            return View(items);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    // upload ảnh vào thư mục wwwroot\\images\\category
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\baiviet", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        model.Image = "/images/baiviet/" + FileName; // gán tên ảnh cho thuộc tinh Image
                    }

                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CategoryId = 3;
				model.Alias = Web_Đồ_An.Models.Models.Common.Filter.FilterChar(model.Title);

				_db.Posts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var item = _db.Posts.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    // upload ảnh vào thư mục wwwroot\\images\\category
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\baiviet", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        model.Image = "/Images/baiviet/" + FileName; // gán tên ảnh cho thuộc tinh Image
                    }

                }
                _db.Posts.Attach(model);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
				model.Alias = Web_Đồ_An.Models.Models.Common.Filter.FilterChar(model.Title);

				_db.Entry(model).Property(x => x.Title).IsModified = true;
                _db.Entry(model).Property(x => x.Description).IsModified = true;
                _db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
             /*  _db.Entry(model).Property(x => x.Image).IsModified = true;*/

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                _db.Posts.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _db.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _db.Posts.Find(Convert.ToInt32(item));
                        _db.Posts.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}