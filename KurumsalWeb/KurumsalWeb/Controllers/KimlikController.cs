using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class KimlikController : Controller
    {
        //veritabanı bağlantısı kur
        KurumsalDBContext db = new KurumsalDBContext();

        // GET: Kimlik
        public ActionResult Index()
        {
            //Kimlik tablosundaki elemanları listeleme


            return View(db.Kimlik.ToList());
        }

        //Kimlik Düzenle Bölgesi
        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            var kimlik = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();

            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]//logoyuda yükleyebilmek için httppostedfile base tipi tanımlanır
        [ValidateInput(false)] //ck editör için
        public ActionResult Edit(int id, Kimlik kimlik,HttpPostedFileBase LogoURL)
        {
            //Model'in var olup olmadığının kontrolü yapılıyor
            if (ModelState.IsValid)
            {
                var k = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();

                if (LogoURL !=null)
                {
                    //Daha önceden var olan resmin olup olmadığını kontrol ediyor
                    if (System.IO.File.Exists(Server.MapPath(kimlik.LogoURL)))
                    {
                        //Varsa eki resmi silme işlemini yaptırdık
                        System.IO.File.Delete(Server.MapPath(kimlik.LogoURL));
                    }

                    //kayıtlı resim yoksa
                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    //string logoname = imginfo.FullName+imginfo.Extension;
                    string logoname=LogoURL.FileName+imginfo.Extension;
                    img.Resize(300, 200); //Resimin yükseklik ve genişlik değeri ayalandı
                    img.Save("~/Uploads/Kimlik/" + logoname); //resim dosyasını olşturduğumuz klasör içine kayıt ettik

                    k.LogoURL = "/Uploads/Kimlik/" + logoname;
                }

                k.Title = kimlik.Title;
                k.Keywords = kimlik.Keywords;
                k.Description = kimlik.Description;
                k.Unvan = kimlik.Unvan;

                //veritabanına kayıt ettik
                db.SaveChanges();

                //kimlik controller kısmındaki index sayfasına yönledirdik
                return RedirectToAction("Index");
            }

            //resim yüklenmediği durumda gideceği yeri belirledik
            return View(kimlik);
        }
    }
}
