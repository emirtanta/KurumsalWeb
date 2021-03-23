using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HakkimizdaController : Controller
    {
        //Veritabanı bağlantısı kurulur
        KurumsalDBContext db = new KurumsalDBContext();

        // GET: Hakkimizda
        public ActionResult Index()
        {
            var h = db.Hakkimizda.ToList();

            return View(h);
        }

        //Hakkımızda Düzenleme bölgesi
        public ActionResult Edit(int id)
        {
            var h = db.Hakkimizda.Where(x => x.HakkimizdaId == id).FirstOrDefault();

            return View(h);
        }

        //Hakkımızda Düzenle Devam Bölgesi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //CK Editör için tanımladık
        public ActionResult Edit(int id,Hakkimizda h)
        {
            if (ModelState.IsValid)
            {
                var hakkimizda = db.Hakkimizda.Where(x => x.HakkimizdaId == id).SingleOrDefault();

                hakkimizda.Aciklama = h.Aciklama;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            
            return View(h);
        }
    }
}