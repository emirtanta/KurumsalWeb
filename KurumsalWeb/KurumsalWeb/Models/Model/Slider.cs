using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KurumsalWeb.Models.Model
{
    [Table("Slider")]
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }

        [DisplayName("Başlık Slider"),StringLength(30,ErrorMessage ="30 Karakter Olmaldır")]
        public string Baslik { get; set; }

        [DisplayName("Slider Açıklama"), StringLength(1500, ErrorMessage = "150 Karakter Olmalıdır")]
        public string Aciklama { get; set; }

        [DisplayName("Slider Resim")]
        public string ResimURL { get; set; }
    }
}