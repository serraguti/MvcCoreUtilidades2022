using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace MvcCoreUtilidades.Helpers
{
    public class HelperMail
    {
        private IConfiguration configuration;

        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private MailMessage ConfigureMail
            (string destinatario, string asunto, string mensaje)
        {
            string from =
                this.configuration.GetValue<string>("MailSettings:User");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(destinatario));
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            return mail;
        }

        private void ConfigureSmtp(MailMessage mail)
        {
            string user =
                this.configuration.GetValue<string>("MailSettings:User");
            string password =
                this.configuration.GetValue<string>("MailSettings:Password");
            string host =
                this.configuration.GetValue<string>("MailSettings:Host");
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = host;
            NetworkCredential credentials =
                new NetworkCredential(user, password);
            client.Credentials = credentials;
            client.Send(mail);
        }

        public void SendMail
            (string destinatario, string asunto, string mensaje)
        {
            MailMessage mail = this.ConfigureMail
                (destinatario, asunto, mensaje);
            mensaje = "<b>Hola que tal</b>";
            //CONFIGURAMOS EL SMTP Y ENVIAR
            this.ConfigureSmtp(mail);
        }

        public void SendMail
            (string destinatario, string asunto
            , string mensaje, string filePath)
        {
            MailMessage mail = this.ConfigureMail
                (destinatario, asunto, mensaje);
            //TENEMOS ADJUNTOS
            Attachment attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment);
            //CONFIGURAMOS EL SMTP Y ENVIAR
            this.ConfigureSmtp(mail);
        }
    }
}
