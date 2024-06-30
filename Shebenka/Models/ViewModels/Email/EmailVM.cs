using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Shebenka.Models.ViewModels.Email
{
    public class EmailVM
    {
        public EmailVM()
        {
        }

        public EmailVM(string email, string subject, string htmlMessage, string name, string telefon, string productname, DateTime Date)
        {
            this.email = email;
            this.subject = subject;
            this.htmlMessage = htmlMessage;
            this.productname = productname;
            this.telefon = telefon;
            this.name = name;
            this.Date = Date;
        }
        public string email { get; set; }
        [DisplayName("Тема сообщения")]
        public string subject { get; set; }
        [DisplayName("Текст")]
        public string htmlMessage { get; set; }
        [DisplayName("Имя")]
        public string name { get; set; }
        [DisplayName("Телефон")]
        public string telefon { get; set; }
        [DisplayName("Названпие товара")]
        public string productname { get; set; }
        [DisplayName("Дата")]
        public DateTime Date { get; set; }

    }
}