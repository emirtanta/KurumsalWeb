using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Admin")] //Tablo adını table değerinden veririz
    public class Admin
    {
        [Key] //Key anahtar kelimesi ile Primary Key tanımlaması yapılır
        public int AdminId { get; set; }

        //Required ile girilmesi zorunlu alan,stringLenght ile girilebilecek karakter sayısı verilir
        [Required,StringLength(50,ErrorMessage ="50 Karakter Olmalıdır")]
        public string Eposta { get; set; }

        //Required ile girilmesi zorunlu alan,stringLenght ile girilebilecek karakter sayısı verilir
        [Required,StringLength(50,ErrorMessage ="50 Karakter Olmalıdır")]
        public string Sifre { get; set; }
        public string Yetki { get; set; }
    }
}