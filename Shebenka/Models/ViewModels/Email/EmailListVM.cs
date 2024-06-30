using Shebenka.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Shebenka.Models.ViewModels.Email
{
    public class EmailListVM
    {
        public EmailListVM()
        {
        }

        public EmailListVM(ZvonkiDTO row)
        {
            Id = row.Id;
            subject = row.subject;
            htmlMessage = row.htmlMessage;
            name = row.name;
            telefon = row.telefon;
            productname = row.productname;
            Date = row.Date;
            
        }
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