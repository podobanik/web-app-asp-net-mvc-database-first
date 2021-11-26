using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAspNetMvcDatabaseFirst.Models.Entities;
using WebAppAspNetMvcDatabaseFirst.ViewModels;

namespace WebAppAspNetMvcDatabaseFirst.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new GosuslugiContext();
            var orders = MappingOrders(db.Orders.ToList());

            return View(orders);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var order = new OrderViewModel();
            return View(order);
        }

        [HttpPost]
        public ActionResult Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new GosuslugiContext();

            var order = new Order();
            MappingOrder(model, order);
            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectPermanent("/Orders/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var order = db.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
                return RedirectPermanent("/Orders/Index");

            db.Orders.Remove(order);
            db.SaveChanges();

            return RedirectPermanent("/Orders/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var order = MappingOrders(db.Orders.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (order == null)
                return RedirectPermanent("/Orders/Index");

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(OrderViewModel model)
        {
            var db = new GosuslugiContext();
            var order = db.Orders.FirstOrDefault(x => x.Id == model.Id);
            if (order == null)
                ModelState.AddModelError("Id", "Книга не найдена");

            if (!ModelState.IsValid)
                return View(model);

            MappingOrder(model, order);

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Orders/Index");
        }

        private void MappingOrder(OrderViewModel sourse, Order destination)
        {
            destination.Procedure = sourse.Procedure;
            destination.Description = sourse.Description;
        }

        private List<OrderViewModel> MappingOrders(List<Order> orders)
        {
            var result = orders.Select(x => new OrderViewModel()
            {
                Id = x.Id,
                Procedure = x.Procedure,
                Description = x.Description,
            }).ToList();

            return result;
        }
    }
}