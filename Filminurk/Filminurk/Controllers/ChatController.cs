using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
