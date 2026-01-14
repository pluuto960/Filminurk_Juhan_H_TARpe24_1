using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OMDbController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
