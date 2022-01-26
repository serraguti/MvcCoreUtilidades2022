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
using MvcCoreUtilidades.Helpers;

namespace MvcCoreUtilidades.Controllers
{
    public class UtilidadesController : Controller
    {
        private HelperMail helperMail;
        private HelperUploadFiles helperUpload;

        public UtilidadesController
            (HelperMail helperMail
            , HelperUploadFiles helperUpload)
        {
            this.helperMail = helperMail;
            this.helperUpload = helperUpload;
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
            if (fichero != null)
            {
                //UTILIZAMOS UPLOADFILES
                string path =
                await this.helperUpload.UploadFileAsync(fichero, Folders.Temp);
                this.helperMail.SendMail(destinatario, asunto, mensaje, path);
            }
            else
            {
                this.helperMail.SendMail(destinatario, asunto, mensaje);
            }
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
            string path =
                await this.helperUpload.UploadFileAsync(fichero, Folders.Uploads);
            ViewBag.FileName = "aq";
            ViewBag.Mensaje = "Fichero subido a " + path;
            return View();
        }
    }
}
