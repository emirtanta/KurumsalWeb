using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Blog")]
    public class Blog
    {
        public int BlogId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string ResimURL { get; set; }

        //Foreign Key Tanımlandı Bu iki değer Kategori tablosu ile 1'e çok ilişki için gereklidir
        public int? KategoriId { get; set; }
        public Kategori Kategori { get; set; }

        public ICollection<Yorum> Yorums { get; set; }
    }
}