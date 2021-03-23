using KurumsalWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList; //Paged List
using PagedList.Mvc; //PagedList.MVC eklendi
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        //veritabanı bağlantısı kur
        private KurumsalDBContext db = new KurumsalDBContext();

        /*sayfalama yapmak için
        * nuget ten PagedListMVC ile PagedList kısımlarını indir
        */
        // GET: Home
        [Route("")]
        [Route("Anasayfa")]//Seo ile Index actionliki anasayfa olarak yapıldı
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x=>x.SliderId));
        }

        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId));
        }

        //Hakkımızda Bölgesi
        [Route("Hakkimizda")]//Seo ile Index actionliki anasayfa olarak yapıldı
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Hakkimizda.SingleOrDefault());
        }

        //Hizmetlerimiz Bölgesi
        [Route("Hizmetlerimiz")]//Seo ile Index actionliki anasayfa olarak yapıldı
        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId));
        }

        //İletişim Sayfası Bölgesi
        [Route("iletisim")]//Seo ile Index actionliki anasayfa olarak yapıldı
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Iletisim.SingleOrDefault());
        }

        //İletişim devam bölgesi
        [HttpPost] //İletişim formundaki bilgiler tanımlandı
        public ActionResult Iletisim(string adsoyad=null,string email=null,string konu=null,string mesaj=null)
        {
            //İletişim formunun gönderilmesi işlemi
            if (adsoyad !=null && email!=null)
            {
                //gamail göndermek için tanımlandı
                WebMail.SmtpServer = "smtp.gmail.com";
                //bağlantının güvenli olması için tanımlanır
                WebMail.EnableSsl = true;

                //kimden mail alındıysa o tanımlanır
                WebMail.UserName = "kurumsalweb01@gmail.com";

                //mail adresinin şifresi 
                WebMail.Password = "Kurumsal36987";

                WebMail.SmtpPort = 587;

                WebMail.Send("kurumsalweb01@gmail.com", konu, email + "-" + mesaj);

                ViewBag.Uyari = "Mesajınız Başarı ile Gönderildi";

                
            }

            else
            {
                ViewBag.Uyari = "Hata Oluştu.Tekrar Deneyiniz";
            }

            return View();
        }

        //Blog Sayfası Bölgesi  PagedList ile sayfalama yaptık
        [Route("BlogPost")]//Seo ile Index actionliki anasayfa olarak yapıldı
        public ActionResult Blog(int Sayfa=1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            return View(db.Blog.Include("Kategori").OrderByDescending(x=>x.BlogId).ToPagedList(Sayfa,5));
        }

        //Kategori Blog Bölgesi
        [Route("BlogPost/{kategoriad}/{id:int}")]
        public ActionResult KategoriBlog(int id,int Sayfa=1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            var b = db.Blog.Include("Kategori").OrderByDescending(x=>x.BlogId).Where(x => x.Kategori.KategoriId == id).ToPagedList(Sayfa,5);

            return View(b);
        }

        //Blog detay bölgesi
        [Route("BlogPost/{baslik}-{id:int}")] //Seo ile oluşturuldu
        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            var b = db.Blog.Include("Kategori").Include("Yorums").Where(x => x.BlogId == id).SingleOrDefault();
            return View(b);
        }

        //Yorum Bölgesi //blogdetay.cshtml'deki yorum bölgesindeki id değerleri(adsoyad,eposta,icerik) tanımlandı 
        public JsonResult YorumYap(string adsoyad,string eposta,string icerik,int blogid)
        {
            if (icerik ==null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            //Yorum veritabanına eklendi
            db.Yorum.Add(new Yorum { AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, BlogId = blogid,Onay=false });

            db.SaveChanges();

            //Response.Redirect("/Home/BlogDetay/" + blogid);

            return Json(false,JsonRequestBehavior.AllowGet);
        }

        public ActionResult BlogKategoriPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            db.Configuration.LazyLoadingEnabled = false;

            return PartialView(db.Kategori.Include("Blogs").ToList().OrderByDescending(x=>x.KategoriAd));
        }

        //Blog Kayıt partial Bölgesi
        public ActionResult BlogKayitPartial()
        {
            return View(db.Blog.ToList().OrderByDescending(x=>x.BlogId));
        }

        //Footer Bölgesi İçin Farklı Tablolardaki Bilgilerin Getirilmesi İçin Kullanıldı
        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();

            //İletişim tablosundaki veriler ekranda gösterilir
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();

            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);

            return PartialView();
        }
    }
}