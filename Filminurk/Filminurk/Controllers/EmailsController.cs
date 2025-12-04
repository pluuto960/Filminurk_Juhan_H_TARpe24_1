using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class EmailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
