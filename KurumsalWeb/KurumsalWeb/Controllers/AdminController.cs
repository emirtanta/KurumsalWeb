using KurumsalWeb.Models;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class AdminController : Controller
    {
        //Veri tabanı bağlantısı kurulur
        KurumsalDBContext db = new KurumsalDBContext();

        // GET: Admin
        [Route("yonetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.BlogSay = db.Blog.Count();

            ViewBag.KategoriSay = db.Kategori.Count();

            ViewBag.HizmetSay = db.Hizmet.Count();

            ViewBag.YorumSay = db.Yorum.Count();

            //Admin template yorum bölgesinde yorumların onay sayısını ve değerini getirir
            ViewBag.YorumOnay = db.Yorum.Where(x => x.Onay == false).Count();

            var sorgu = db.Kategori.ToList();

            return View(sorgu);
        }

        //Codefirst ile veri tabanı eklemek için aşağıdaki adımlar yapılır
        //1)Tools kısmından manage nuget içindeki package manager seçilir
        //2)Pm yarine Enable-Migrations komutu yazılıp ardından komut bittikten sonra build edilir
        //3)Pm kısmına update-database komutu yazılarak veritabanı oluşturulur
        [Route("yonetimpaneli/giris")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin,string sifre)
        {
            //oluşturulan şifreyi md5 şifreleme algoritmasına dönüştürür
            var md5pass = Crypto.Hash(sifre, "MD5");

            var login = db.Admin.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();

            if (login.Eposta==admin.Eposta && login.Sifre==md5pass)
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;

                //Belli Bölgelere Belirli Yetkideki Kullanıcıların Girebilmesi için tanımlandı
                Session["yetki"] = login.Yetki;

                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Uyari = "Kullanıcı adı yada şifre yanlış";

            return View(admin);
        }

        //Log out Bölgesi
        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;

            

            //sessionları siliyoruz
            Session.Abandon();

            return RedirectToAction("Login","Admin");
        }

        //Şifremi Unuttum Bölgesi
        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        //Şifremi Unuttum Devam Bölgesi
        [HttpPost]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();

            //Şifreyi Mail Adresine Gönderme İşlemi
            if (mail !=null)
            {
                Random rnd = new Random();
                int yenisifre = rnd.Next();

                Admin sifre = new Admin();
                mail.Sifre = Crypto.Hash(Convert.ToString(yenisifre), "MD5");

                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "emirdeneme41@gmail.com"; //ana mail adresi
                WebMail.Password = "Et19901990";
                WebMail.SmtpPort = 587;
                WebMail.Send(eposta, "Admin Panel Giriş Şifreniz", "Şifreniz: "+yenisifre);
                //WebMail.Send("emirtanta41@gmail.com", konu, email + "-" + mesaj);

                ViewBag.Uyari = "Şifrenin Mail Adresinize Gönderildi";
            }

            else
            {
                ViewBag.Uyari = "Hata oluştu.Tekrar deneyiniz";
            }

            return View();
        }

        public ActionResult Adminler()
        {
            return View(db.Admin.ToList());
        }

        //Admin Ekle Bölgesi
        public ActionResult Create()
        {
            return View();
        }

        //Admin Ekle Devam Bölgesi
        [HttpPost]
        public ActionResult Create(Admin admin,string sifre,string eposta)
        {


            if (ModelState.IsValid)
            {
                //Şifrenin şifreli şekilde olmasını sağladık
                admin.Sifre = Crypto.Hash(sifre, "MD5");

                db.Admin.Add(admin);

                db.SaveChanges();

                return RedirectToAction("Adminler");
            }

            return View(admin);
        }

        //Admin Güncelleme İşlemi
        public ActionResult Edit(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();

            return View(a);
        }

        //Admin Güncelleme Devam
        [HttpPost]
        public ActionResult Edit(int id,Admin admin,string sifre,string eposta)
        {
            

            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();

                a.Sifre = Crypto.Hash(sifre, "MD5");
                a.Eposta = admin.Eposta;
                a.Yetki = admin.Yetki;

                db.SaveChanges();

                return RedirectToAction("Adminler");
            }

            return View(admin);
        }

        //Adminler Sil Bölgesi
        public ActionResult Delete(int id)
        {

            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();

            if (a!=null)
            {
                db.Admin.Remove(a);

                db.SaveChanges();

                return RedirectToAction("Adminler");
            }

            return View();
        }
    }
}