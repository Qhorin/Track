using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Track.Controllers;
using Track.ViewModels;

namespace Track.Models
{
    public class TransactionController : Controller
    {
        private TrackDb db = new TrackDb();

        //
        // GET: /Transaction/

        public ActionResult Index()
        {

            //var data = db.Transactions.ToList();
            //var serializer = new JavaScriptSerializer();
            //var viewModel = serializer.Serialize(data);

            //return Json(viewModel, JsonRequestBehavior.AllowGet);
            return View();
            
        }

        //
        // GET: /Transaction/Details/5

        public ActionResult Details(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // GET: /Transaction/Create

        public ActionResult Create()
        {
            var model = new Transaction();
            model.Timestamp = DateTime.Now;

            PopulateStoreDropdown();
            return View(model);
        }

        //
        // POST: /Transaction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        //
        // GET: /Transaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);//.Include(x => x.StoreId).Where(y => y.TransactionId == id).Single();
            //var transaction = db.Transactions.Include(x => x.TransactionItems).Where(y => y.TransactionId == id).Single();

            var model = new TransactionEditViewModel();
            model.Transaction = transaction;
            model.TransactionItems = transaction.TransactionItems; //db.TransactionItems.Where(x => x.TransactionId == id);


            if (transaction == null)
            {
                return HttpNotFound();
            }

            //var storeQry = from s in db.Stores
            //               orderby s.Name
            //               select s;

            PopulateStoreDropdown(transaction.StoreId);

            return View(model);
        }

        //
        // POST: /Transaction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TransactionEditViewModel model) //Transaction transaction, ICollection<TransactionItem> transactionItems)
        {

            foreach (var modelStateValue in ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    // Do something useful with these properties
                    var errorMessage = error.ErrorMessage;
                    var exception = error.Exception;
                }
            }

            var transaction = model.Transaction;
            var transactionItems = model.TransactionItems;
            db.Entry(transaction).State = EntityState.Modified;

            

            if (true) //(ModelState.IsValid)
            {

                foreach (var item in transactionItems)
                {

                    //For this item, see if it already exists in the database
                    var existingItem = db.TransactionItems
                        .Where(x => x.TransactionItemId == item.TransactionItemId)
                        .SingleOrDefault();

                    // Yes, item exists, so update scalar properties of child item
                    if (existingItem != null)
                    {
                        var child = db.Entry(existingItem);
                        child.CurrentValues.SetValues(item);
                    }
                    else //item is new
                    {

                        item.TransactionItemId = 0;
                        db.Entry(transaction).Collection(x => x.TransactionItems).Load();
                        //db.Entry(transaction).Reference(x => x.TransactionItems).Load();
                        transaction.TransactionItems.Add(item); //I think this isn't right. This may blow up later.
                    }
                }
            

                var totalPrice = CalculateTotalPrice(transactionItems);
                transaction.TotalPrice = totalPrice;
                

                
                //db.Entry(transaction).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateStoreDropdown(transaction.StoreId);
            return View(model);
        }

        //
        // GET: /Transaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // POST: /Transaction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
        private void PopulateStoreDropdown(int storeId = 0)
        {
            //Populate Store dropdown
            ViewBag.StoreId = (List<SelectListItem>)new SelectList(db.Stores.OrderBy(x => x.Name), "Id", "Name", storeId).ToList();
        }

        private decimal CalculateTotalPrice(ICollection<TransactionItem> items)
        {
            var total = items.Sum(x => x.Price);
            return total;
        }

        public ActionResult AddNew()
        {
            var model = new TransactionItem();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddNew", model);
            }

            return View();//not used?
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Transaction transaction = db.Transactions.Find(id);//.Include(x => x.StoreId).Where(y => y.TransactionId == id).Single();
        //    //var transaction = db.Transactions.Include(x => x.TransactionItems).Where(y => y.TransactionId == id).Single();

        //    var model = new TransactionEditViewModel();
        //    model.Transaction = transaction;
        //    model.TransactionItems = db.TransactionItems.Where(x => x.TransactionId == id);


        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //var storeQry = from s in db.Stores
        //    //               orderby s.Name
        //    //               select s;

        //    PopulateStoreDropdown(transaction.StoreId);

        //    return View("Test", model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(IList<TransactionItem> TransactionItems)
        //{


        //    RedirectToAction("Index");
            
        //    return View("Index");
        //}

        public ActionResult TransactionItemRow()
        {
            return PartialView("_TransactionItem");
        }

        public ActionResult LoadTransactions()
        {

            var data = db.Transactions.ToList();
            //var viewModel = new ViewModel();
            var serializer = new JavaScriptSerializer();
            var viewModel = serializer.Serialize(data);

            var javascriptJson = JsonConvert.SerializeObject(data, new IsoDateTimeConverter());

            return Json(javascriptJson, JsonRequestBehavior.AllowGet);
        }
    }
}