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
    public class BlogController : Controller
    {
        //veritabanı bağlantısı kur
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Blog
        public ActionResult Index()
        {
            //1. yöntem
            //var b = db.Blog.ToList();
            //return View(b);

            //2. yöntem
            db.Configuration.LazyLoadingEnabled = false; //Kategori tablosundaki değerlerin gelmesi  için

            return View(db.Blog.Include("Kategori").ToList().OrderByDescending(x=>x.BlogId));
        }

        //Blog Ekle Bölgesi
        public ActionResult Create()
        {
            //Kategorilerin almaak için viewbag kullanılır selectlist kullanılır
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");

            return View();
        }

        //Blog Ekle Devam Bölgesi
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog,HttpPostedFileBase ResimURL)
        {
            if (ResimURL != null)
            {
                //kayıtlı resim yoksa
                WebImage img = new WebImage(ResimURL.InputStream);
                FileInfo imginfo = new FileInfo(ResimURL.FileName);

                //string logoname = imginfo.FullName+imginfo.Extension;
                string blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Resize(600, 400); //Resimin yükseklik ve genişlik değeri ayalandı
                img.Save("~/Uploads/Blog/" + blogimgname); //resim dosyasını olşturduğumuz klasör içine kayıt ettik

                blog.ResimURL = "/Uploads/Blog/" + blogimgname;
            }

            //veritabanına kayıt ekleniyor
            db.Blog.Add(blog);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Blog Düzenle Bölgesi
        public ActionResult Edit(int id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }

            var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();

            if (b==null)
            {
                return HttpNotFound();
            }

            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd", b.KategoriId);

            return View(b);
        }
        //b.kategoriId (viewbag)

        //Blog Düzenle Devam Bölgesi
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id,Blog blog,HttpPostedFileBase ResimURL)
        {
            //Model'in doğruluğunun kontrolü
            if (ModelState.IsValid)
            {
                var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();

                //Resim Yükleme İşlemi
                if (ResimURL !=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimURL));
                    }

                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + blogimgname);

                    b.ResimURL = "/Uploads/Blog/" + blogimgname;
                }

                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.KategoriId = blog.KategoriId;
                //b.KategoriId = blog.KategoriId;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(blog);
        }
        

        //Blog Sil Bölgesi
        public ActionResult Delete(int id)
        {
            var b = db.Blog.Find(id);

            if (b==null)
            {
                return HttpNotFound();
            }

            //Kayıtlı Resmi de Silmek için
            if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(b.ResimURL));
            }

            db.Blog.Remove(b);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}