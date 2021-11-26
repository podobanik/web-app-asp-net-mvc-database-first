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
    public class ClientTypesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new GosuslugiContext();
            var clientTypes = MappingClientTypes(db.ClientTypes.ToList());

            return View(clientTypes);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var clientType = new ClientTypeViewModel();
            return View(clientType);
        }

        [HttpPost]
        public ActionResult Create(ClientTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new GosuslugiContext();
            var clientType = new ClientType();
            MappingClientType(model, clientType);
            db.ClientTypes.Add(clientType);
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var clientType = db.ClientTypes.FirstOrDefault(x => x.Id == id);
            if (clientType == null)
                return RedirectPermanent("/ClientTypes/Index");

            db.ClientTypes.Remove(clientType);
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var clientType = MappingClientTypes(db.ClientTypes.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (clientType == null)
                return RedirectPermanent("/ClientTypes/Index");

            return View(clientType);
        }

        [HttpPost]
        public ActionResult Edit(ClientTypeViewModel model)
        {
            var db = new GosuslugiContext();
            var clientType = db.ClientTypes.FirstOrDefault(x => x.Id == model.Id);
            if (clientType == null)
                ModelState.AddModelError("Id", "Жанр не найден");

            if (!ModelState.IsValid)
                return View(model);

            MappingClientType(model, clientType);

            db.Entry(clientType).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }

        private void MappingClientType(ClientTypeViewModel sourse, ClientType destination)
        {
            destination.Name = sourse.Name;
        }

        private List<ClientTypeViewModel> MappingClientTypes(List<ClientType> clientTypes)
        {
            var result = clientTypes.Select(x => new ClientTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return result;
        }
    }
}