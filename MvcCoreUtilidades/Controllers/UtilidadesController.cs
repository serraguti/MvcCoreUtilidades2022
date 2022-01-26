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

        public IActionResult CifradoEficiente() {
            return View();
        }

        [HttpPost]
        public IActionResult CifradoEficiente
            (string contenido, string resultado, string accion)
        {
            string response = "";
            if (accion.ToLower() == "cifrar")
            {
                response =
                    HelperCryptography.EncriptarContenido(contenido, false);
                ViewData["TEXTOCIFRADO"] = response;
            }
            else if (accion.ToLower() == "comparar")
            {
                response =
                    HelperCryptography.EncriptarContenido(contenido, true);
                if (response != resultado)
                {
                    ViewData["MENSAJE"] = "<h1 style='color:red'>No son iguales</h1>";
                }
                else
                {
                    ViewData["MENSAJE"] = 
                        "<h1 style='color:blue'>PASSWORD CORRECTA</h1>";
                }
            }
            return View();
        }

        public IActionResult CifradoBasico()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CifradoBasico
            (string contenido, string resultado, string accion)
        {
            //CUANDO ENTRE AQUI CIFRAMOS EL CONTENIDO
            string response = 
                HelperCryptography.EncriptarTextoBasico(contenido);
            if (accion.ToLower() == "cifrar")
            {
                ViewData["TEXTOCIFRADO"] = response;
            }else if (accion.ToLower() == "comparar")
            {
                //COMPARAMOS EL RESULTADO (CIFRADO) CON EL CONTENIDO CIFRADO
                if (response != resultado)
                {
                    ViewData["MENSAJE"] = "Los valores no coinciden";
                }
                else
                {
                    ViewData["MENSAJE"] = "Mismos valores!!!";
                }
            }
            return View();
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
