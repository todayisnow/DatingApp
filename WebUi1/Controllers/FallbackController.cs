using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WebUi1.Controllers
{
    public class FallbackController : Controller  //mvc controller
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), 
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}