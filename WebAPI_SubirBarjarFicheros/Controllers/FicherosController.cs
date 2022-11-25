using Microsoft.AspNetCore.Mvc;

namespace WebAPI_SubirBarjarFicheros.Controllers
{
    public class FicherosController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public IActionResult Index()
        {
            return View();
        }

        public FicherosController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost, Route("subirDocumento")]
        public ActionResult subirDocumento([FromForm] IFormFile fichero)
        {
            try
            {
                // Obtener ruta de destino
                string rutaDestino = _webHostEnvironment.ContentRootPath + "\\FicherosSubidos";
                if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
                string rutaDestinoCompleta = Path.Combine(rutaDestino, fichero.FileName);

                // Se valida si la variable "fichero" tiene algún archivo
                if (fichero.Length > 0)
                {
                    // Se crea una variable FileStream para cargarlo en la ruta definida
                    using (var stream = new FileStream(rutaDestinoCompleta, FileMode.Create))
                    {
                        fichero.CopyTo(stream);
                    }
                }

                // Respuesta
                return Ok("El documento se ha subido correctamente.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost, Route("subirDocumentoBase64")]
        public ActionResult subirDocumentoBase64([FromForm] string base64, [FromForm] string nombreFichero)
        {
            try
            {
                // Obtener ruta de destino
                string rutaDestino = _webHostEnvironment.ContentRootPath + "\\FicherosSubidos"; ;
                if (!Directory.Exists(rutaDestino)) Directory.CreateDirectory(rutaDestino);
                string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);

                // Grabar base64 como fichero
                byte[] documento = Convert.FromBase64String(base64);
                System.IO.File.WriteAllBytes(rutaDestinoCompleta, documento);

                // Respuesta
                return Ok("El documento se ha subido correctamente.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost("bajarDocumento")]
        public ActionResult BajarDocumento([FromForm] string nombreFichero)
        {
            try
            {
                string rutaDestino = _webHostEnvironment.ContentRootPath + "\\FicherosSubidos";
                string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);
                byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
                return File(bytes, "application/octet-stream", nombreFichero);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


        [HttpPost("bajarDocumentoBase64")]
        public ActionResult BajarDocumentoBase64([FromForm] string nombreFichero)
        {
            try
            {
                string rutaDestino = _webHostEnvironment.ContentRootPath + "\\FicherosSubidos";
                string rutaDestinoCompleta = Path.Combine(rutaDestino, nombreFichero);

                byte[] bytes = System.IO.File.ReadAllBytes(rutaDestinoCompleta);
                var base64String = Convert.ToBase64String(bytes);
                return Ok(base64String);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



    }
}
