using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcCoreUtilidades.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace MvcCoreUtilidades.Controllers
{
    public class UtilidadesController : Controller
    {
        private PathProvider pathProvider;
        private IConfiguration Configuration;

        public UtilidadesController
            (PathProvider pathProvider
            , IConfiguration configuration)
        {
            this.pathProvider = pathProvider;
            this.Configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail
            (string destinatario, string asunto
            , string mensaje, IFormFile fichero)
        {
            MailMessage mail = new MailMessage();
            string user = this.Configuration.GetValue<string>("MailSettings:user");
            //LA CUENTA DE SALIDA DEBE SER LA MISMA QUE TENEMOS
            //EN APPSETTINGS
            mail.From = new MailAddress(user);
            //LOS DESTINATARIOS SON UNA COLECCION
            mail.To.Add(new MailAddress(destinatario));
            mail.Subject = asunto;
            mail.Body = mensaje;
            //PERO PODRIAMOS ENVIAR UN FORMATO HTML EN EL MENSAJE
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            //COMPROBAMOS SI TENEMOS ADJUNTOS
            if (fichero != null)
            {
                //TENEMOS ADJUNTOS
                string fileName = fichero.FileName;
                string path =
                    this.pathProvider.MapPath(fileName, Folders.Temp);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await fichero.CopyToAsync(stream);
                }
                Attachment attachment = new Attachment(path);
                mail.Attachments.Add(attachment);
            }
            string host = this.Configuration.GetValue<string>("MailSettings:Host");
            string password = 
                this.Configuration.GetValue<string>("MailSettings:Password");
            //CONFIGURAMOS EL CLIENTE SMTP PARA ENVIAR EL CORREO
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            NetworkCredential credentials =
                new NetworkCredential(user, password);
            client.Credentials = credentials;
            client.Send(mail);
            ViewData["MENSAJE"] = "Mail enviado correctamente";
            return View();
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile fichero)
        {
            string fileName = fichero.FileName;
            string path = this.pathProvider.MapPath(fileName, Folders.Uploads);
            //CREAMOS EL FICHERO Y LO LEEMOS COMO UN STREAM
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            ViewBag.FileName = fileName;
            ViewBag.Mensaje = "Fichero subido a " + path;
            return View();
        }
    }
}
