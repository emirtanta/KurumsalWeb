using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.DataContext
{
    public class KurumsalDBContext:DbContext
    {
        //base bölgesine "" arasına webconfig de add name yerine yazılacak değer ile aynı adı yazmamız gerekir
        public KurumsalDBContext():base("KurumsalWebDB")
        {

        }

        //veritabanına tabloları eklemek için
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Hakkimizda> Hakkimizda { get; set; }
        public DbSet<Hizmet> Hizmet { get; set; }
        public DbSet<Iletisim> Iletisim { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<Kimlik> Kimlik { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Yorum> Yorum { get; set; }

        /*veritabanına farklı bir tablo ekleme için
         * ilk önce dbset olarak ilgili tablo oluşturulur sonra
         * package manager console ekranı açılır
         * ardından Enable-Migrations -force komutu yazılır ve enter tuşuna basılır 
         * ardından Migrations klasöründeki COnfiguration kısmındaki AutomaticMigrationsEnabled false
         * yerine true yapılır
         * ardından update-database -force komutu yazılıp entere basılır
         * */
    }
}