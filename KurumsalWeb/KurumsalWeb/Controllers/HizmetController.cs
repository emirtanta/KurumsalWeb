using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    //Hizmet Controllerdeki İşlemleri manuel olarak yaptık

    public class HizmetController : Controller
    {
        //Veritabanı bağlantısı kur
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Hizmet
        public ActionResult Index()
        {
            return View(db.Hizmet.ToList());
        }

        //Hizmet Ekle Bölgesi
        public ActionResult Create()
        {
            return View();
        }

        //Hizmet Ekle Devam Bölgesi
        [HttpPost]
        [ValidateInput(false)] //textarea işlemi için ck editör
        public ActionResult Create(Hizmet hizmet,HttpPostedFileBase ResimURL)
        {
            //Model'in doğruluğunun kontrolü
            if (ModelState.IsValid)
            {

                //Resim Yükleme İşlemi
                if (ResimURL != null)
                {

                    //kayıtlı resim yoksa
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    //string logoname = imginfo.FullName+imginfo.Extension;
                    string logoname = ResimURL.FileName + imginfo.Extension;
                    img.Resize(500, 500); //Resimin yükseklik ve genişlik değeri ayalandı
                    img.Save("~/Uploads/Hizmet/" + logoname); //resim dosyasını olşturduğumuz klasör içine kayıt ettik

                    hizmet.ResimURL = "/Uploads/Hizmet/" + logoname;
                }


                //Hizmet tablosuna hizmet ekleniyor
                db.Hizmet.Add(hizmet);

                //veritabanına kaydediliyor
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(hizmet);
        }

        //Hizmet Düzenle Bölgesi
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                ViewBag.Uyari = "Güncellenecek Hizmet Bulunamadı";
            }

            var hizmet = db.Hizmet.Find(id);

            if (hizmet==null)
            {
                return HttpNotFound();
            }

            return View(hizmet);
        }

        //Hizmet Düzenle Devam Bölgesi
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id,Hizmet hizmet,HttpPostedFileBase ResimURL)
        {
           

            //Model'in doğruluğunun kontrolü
            if (ModelState.IsValid)
            {
                var h = db.Hizmet.Where(x => x.HizmetId == id).SingleOrDefault();

                //Resim Düzenleme İşlemi
                if (ResimURL !=null)
                {

                    //Daha önceden var olan resmin olup olmadığını kontrol ediyor
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))
                    {
                        //Varsa eki resmi silme işlemini yaptırdık
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }

                    //kayıtlı resim yoksa
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    //string logoname = imginfo.FullName+imginfo.Extension;
                    string hizmetname = ResimURL.FileName + imginfo.Extension;
                    img.Resize(500, 500); //Resimin yükseklik ve genişlik değeri ayalandı
                    img.Save("~/Uploads/Hizmet/" + hizmetname); //resim dosyasını olşturduğumuz klasör içine kayıt ettik

                    h.ResimURL = "/Uploads/Hizmet/" + hizmetname;
                }
                //Veritabanındaki bilgiler hizmet modeline aktarılıyor
                h.Baslik = hizmet.Baslik;
                h.Aciklama = hizmet.Aciklama;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        //Hizmet Sil Bölgesi
        //public ActionResult Delete(int id)
        //{
        //    if (id==null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var h = db.Hizmet.Find(id);

        //    if (h==null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //Hizmet tablosundan veri silinip veritabanına kaydediliyor
        //    db.Hizmet.Remove(h);
        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        // GET: Iletisim/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hizmet h = db.Hizmet.Find(id);
            if (h == null)
            {
                return HttpNotFound();
            }
            return View(h);
        }

        // POST: Iletisim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hizmet h = db.Hizmet.Find(id);
            db.Hizmet.Remove(h);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}