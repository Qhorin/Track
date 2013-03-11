using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View(db.Transactions.ToList());
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
            model.TransactionItem = db.TransactionItems.Where(x => x.TransactionId == id);


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
        public ActionResult Edit(Transaction transaction)
        {
        
            var current = db.Transactions.Include(x => x.TransactionItems).Single(x => x.TransactionId == transaction.TransactionId);
        
           
            if (ModelState.IsValid)
            {
                foreach (var item in transaction.TransactionItems){

                    var originalChildItem = current.TransactionItems
                        .Where(x => x.TransactionItemId == item.TransactionItemId)
                        .SingleOrDefault();

                    // Yes, update scalar properties of child item
                    if (originalChildItem != null)
                    {
                        var child = db.Entry(originalChildItem);
                        child.CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.TransactionItemId = 0;
                        current.TransactionItems.Add(item);
                    }
                }

                var totalPrice = CalculateTotalPrice(transaction.TransactionItems);
                current.TotalPrice = totalPrice;
                UpdateModel(current);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
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
            ViewBag.StoreId = new SelectList(db.Stores.OrderBy(x => x.Name), "Id", "Name", storeId);
        }

        private decimal CalculateTotalPrice(ICollection<TransactionItem> items)
        {
            var total = items.Sum(x => x.Price);
            return total;
        }
    }
}