using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shebenka.Models.Data
{
    [Table("TableZvonki")]
    public class ZvonkiDTO
    {
        public int Id { get; set; }
        [DisplayName("Тема сообщения")]
        public string subject { get; set; }
        [DisplayName("Текст")]
        public string htmlMessage { get; set; }
        [DisplayName("Имя")]
        public string name { get; set; }
        [DisplayName("Телефон")]
        public string telefon { get; set; }
        [DisplayName("Название товара")]
        public string productname { get; set; }
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
}