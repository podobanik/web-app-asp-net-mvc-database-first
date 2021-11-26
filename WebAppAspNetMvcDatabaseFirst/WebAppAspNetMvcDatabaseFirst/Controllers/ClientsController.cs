using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebAppAspNetMvcDatabaseFirst.Models.Entities;
using WebAppAspNetMvcDatabaseFirst.Models.Enums;
using WebAppAspNetMvcDatabaseFirst.ViewModels;

namespace WebAppAspNetMvcDatabaseFirst.Controllers
{
    public class ClientsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new GosuslugiContext();
            var clients = MappingClients(db.Clients.ToList());

            return View(clients);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var client = new ClientViewModel();
            return View(client);
        }

        [HttpPost]
        public ActionResult Create(ClientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var db = new GosuslugiContext();
            var client = new Client();
            
            MappingClient(model, client, db);

            db.Clients.Add(client);
            db.SaveChanges();

            return RedirectPermanent("/Clients/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Clients/Index");

            db.Clients.Remove(client);
            db.SaveChanges();

            return RedirectPermanent("/Clients/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var client = MappingClients(db.Clients.Where(x => x.Id == id).ToList()).FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Clients/Index");

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(ClientViewModel model)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == model.Id);
            if (client == null)
                ModelState.AddModelError("Id", "Книга не найдена");

            if (!ModelState.IsValid)
                return View(model);

            MappingClient(model, client, db);

            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/Clients/Index");
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            var db = new GosuslugiContext();
            var image = db.Documents.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                FileStream fs = System.IO.File.OpenRead(Server.MapPath(@"~/Content/Images/not-foto.png"));
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                fs.Close();

                return File(new MemoryStream(fileData), "image/jpeg");
            }

            return File(new MemoryStream(image.Data), image.ContentType);
        }

        private void MappingClient(ClientViewModel sourse, Client destination, GosuslugiContext db)
        {
            destination.Name = sourse.Name;
            destination.Surname = sourse.Surname;
            destination.Age = sourse.Age;
            destination.Birthday = sourse.Birthday;
            destination.ClientTypeId = sourse.ClientTypeId;
            destination.ClientType = sourse.ClientType;
            destination.Gender = (int)sourse.Gender;

            if (destination.Orders != null)
                destination.Orders.Clear();

            if (sourse.OrderIds != null && sourse.OrderIds.Any())
                destination.Orders = db.Orders.Where(s => sourse.OrderIds.Contains(s.Id)).ToList();

            if (sourse.DocumentFile != null)
            {
                var image = db.Documents.FirstOrDefault(x => x.Id == sourse.Id);
                if (image != null)
                    db.Documents.Remove(image);

                var data = new byte[sourse.DocumentFile.ContentLength];
                sourse.DocumentFile.InputStream.Read(data, 0, sourse.DocumentFile.ContentLength);

                destination.Document = new Document()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = sourse.DocumentFile.ContentType,
                    FileName = sourse.DocumentFile.FileName
                };
            }
        }

        private List<ClientViewModel> MappingClients(List<Client> clients)
        {
            var result = clients.Select(x => new ClientViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                ClientType = x.ClientType,
                Age = x.Age,
                Birthday = x.Birthday,
                Gender = (Gender)x.Gender,
                ClientTypeId = x.ClientTypeId,
                Document = x.Document,
                Orders = x.Orders
            }).ToList();

            return result;
        }
    }
}