using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LostFound.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Globalization;
using System.IO;

namespace LostFound.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index(string type)
        {
            if(string.IsNullOrEmpty(type) || type.Equals("Found") ){
                ViewBag.IsLost = 0; //False
            }
            else if (type.Equals("Lost")) {
                ViewBag.IsLost = 1; //True
            }
            else {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetIInitialtems(bool lost)
        {
            var list = new List<Item>();
            var items = db.Items.Where(x => x.Lost == lost).ToList();
            list.AddRange(items);

            var model = new IndexItemViewModel()
            {
                Items = list,
                Categories = db.Categories.ToList(),
                Counties = db.Counties.ToList(),
                IsLost = lost,
                SearchString = "",
                FromDate = "",
                SelectedCategories = new List<string>(),
                SelectedCounties = new List<string>()
            };

            return Json(model);
        }

        [HttpPost]
        public ActionResult GetItems(IndexItemViewModel model) 
        {
            //Start query
            var Items = from item in db.Items
                        select item;

            //Lost or found
            Items = Items.Where(p => p.Lost == model.IsLost);

            //From date on found item
            DateTime fromDate;
            if(!model.IsLost && DateTime.TryParse(model.FromDate, out fromDate))
                Items = Items.Where(p => DateTime.Compare(fromDate, p.FoundDate) <= 0);

            //Search string
            if (!string.IsNullOrEmpty(model.SearchString))
                Items = Items.Where(p => p.Name.Contains(model.SearchString) || p.Description.Contains(model.SearchString));

            //Categories
            if (model.SelectedCategories.Any())
                Items = Items.Where(p => model.SelectedCategories.Contains(p.Category.Name));

            //Counties
            if (model.SelectedCounties.Any())
                Items = Items.Where(p => model.SelectedCounties.Contains(p.County.Name));

            var test = 0;

            //Add items to model
            model.Items = Items.ToList();
            
            //Return model
            return Json(model);
        }

        public ActionResult MyItems()
        {
            var userId = User.Identity.GetUserId();

            if (userId != null)
            {
                var foundItemsList = new List<Item>();
                var foundItems = db.Items.Where(x => x.UserId == userId && x.Lost == false).ToList();
                foundItemsList.AddRange(foundItems);

                var lostItemsList = new List<Item>();
                var lostItems = db.Items.Where(x => x.UserId == userId && x.Lost == true).ToList();
                lostItemsList.AddRange(lostItems);

                MyItemsViewModel viewModel = new MyItemsViewModel()
                {
                    FoundItems = foundItems,
                    LostItems = lostItems
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize]
        public ActionResult Create()
        {
            var model = new NewItemViewModel() 
            {
                Item = new Item(),
                DateFound = DateTime.Now.ToString("dd/MM/yyyy"),
                DateLostFrom = (DateTime.Now.AddDays(-7)).ToString("dd/MM/yyyy"),
                DateLostTo = DateTime.Now.ToString("dd/MM/yyyy"),
                Categories = db.Categories.OrderBy(x=> x.Name).ToList()
            };
            return View(model);
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Save image
                if (model.Image != null && model.Image.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + model.Image.FileName;

                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    model.Item.ImgUrl = fileName;
                    model.Image.SaveAs(path);
                }

                //Get category
                var category = db.Categories.FirstOrDefault(x=> x.ID == model.Item.Category.ID);
                model.Item.Category = category;

                //Get county
                var countyName = model.Item.County.Name;
                model.Item.County = null;
                var county = db.Counties.FirstOrDefault(x => x.Name.Equals(countyName));
                model.Item.County = county != null ? county : null;

                //Parse datetime
                var dtfi = new DateTimeFormatInfo { ShortDatePattern = "dd.MM.yyyy" };
                model.Item.FoundDate = Convert.ToDateTime(model.DateFound, dtfi);
                model.Item.LostDateFrom = Convert.ToDateTime(model.DateLostFrom, dtfi);
                model.Item.LostDateTo = Convert.ToDateTime(model.DateLostTo, dtfi);

                //Add user to the model
                var userId = User.Identity.GetUserId();
                model.Item.UserId = userId;

                //Save item
                var item = model.Item;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model.Item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = db.Items.Find(id);

            if (item == null)
            {
                return HttpNotFound();
            }

            var model = new NewItemViewModel()
            {
                Item = item,
                DateFound = item.FoundDate.ToString("dd/MM/yyyy"),
                DateLostFrom = item.LostDateFrom.ToString("dd/MM/yyyy"),
                DateLostTo = item.LostDateTo.ToString("dd/MM/yyyy"),
                Categories = db.Categories.OrderBy(x => x.Name).ToList()
            };

            return View(model);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Save image
                if (model.Image != null && model.Image.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + model.Image.FileName;

                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    model.Item.ImgUrl = fileName;
                    model.Image.SaveAs(path);
                }

                //Get category
                var category = db.Categories.FirstOrDefault(x => x.ID == model.Item.Category.ID);
                model.Item.Category = category;

                //Get county
                var countyName = model.Item.County.Name;
                model.Item.County = null;
                var county = db.Counties.FirstOrDefault(x => x.Name.Equals(countyName));
                model.Item.County = county != null ? county : null;

                //Parse datetime
                var dtfi = new DateTimeFormatInfo { ShortDatePattern = "dd.MM.yyyy" };
                model.Item.FoundDate = Convert.ToDateTime(model.DateFound, dtfi);
                model.Item.LostDateFrom = Convert.ToDateTime(model.DateLostFrom, dtfi);
                model.Item.LostDateTo = Convert.ToDateTime(model.DateLostTo, dtfi);

                //Add user to the model
                var userId = User.Identity.GetUserId();
                model.Item.UserId = userId;

                //Save item
                var item = model.Item;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = item.Id });
            }

            return View(model);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
