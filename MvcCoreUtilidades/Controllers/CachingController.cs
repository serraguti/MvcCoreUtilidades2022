using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreUtilidades.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache memoryCache;

        public CachingController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult MemoriaPersonalizada(int? tiempo)
        {
            if (tiempo == null)
            {
                tiempo = 5;
            }
            string fecha =
                DateTime.Now.ToLongDateString()
                + ", " + DateTime.Now.ToLongTimeString();
            if (this.memoryCache.Get("FECHA") == null)
            {
                MemoryCacheEntryOptions options =
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(
                        TimeSpan.FromSeconds(tiempo.Value));
                //LE PASAMOS LAS OPCIONES COMO TERCER PARAMETRO
                this.memoryCache.Set("FECHA", fecha, options);
                ViewData["MENSAJE"] = "Almacenando en Cache " + 
                    tiempo.Value + " segundos.";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");
            }
            else
            {
                //RECUPERAMOS LA FECHA DEL MEMORY
                fecha = this.memoryCache.Get("FECHA").ToString();
                ViewData["FECHA"] = fecha;
                ViewData["MENSAJE"] = "Recuperando fecha de Cache";
            }
            return View();
        }

        [ResponseCache(Duration = 15)]
        public IActionResult MemoriaDistribuida()
        {
            string fecha = DateTime.Now.ToLongDateString()
                + ", " + DateTime.Now.ToLongTimeString();
            ViewData["FECHA"] = fecha;
            return View();
        }
    }
}
