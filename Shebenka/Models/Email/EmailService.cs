using MimeKit;
using Shebenka.Models.ViewModels.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shebenka.Models.Email
{
    public class EmailService
    {
        public void SendEmailCustom(EmailVM model)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(Properties.Settings.Default.Company, Properties.Settings.Default.EmailNameProd)); //отправитель сообщения
                message.To.Add(new MailboxAddress("", model.email)); //адресат сообщения
                message.Subject = model.subject; //тема сообщения

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Имя: " +  model.name + "<br /> Телефон: " + model.telefon + "<br /> Товар с которого пришел заказ: " + model.productname + "<br /> Сообщение:  " + model.htmlMessage + "<br /> Дата:  " + model.Date
                };
                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(Properties.Settings.Default.HostSmtp, Properties.Settings.Default.EmailPort, Properties.Settings.Default.EmailUseSsl); //либо использум порт 465 587

                    client.Authenticate(Properties.Settings.Default.EmailNameProd, Properties.Settings.Default.EmailPassProd); //логин-пароль от аккаунта
                    client.Send(message);
                    client.Disconnect(true);
                    //logger.LogInformation("Сообщение отправлено успешно!");
                   // Console.WriteLine("Сообщение отправлено успешно!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);

            }
        }
    }
}