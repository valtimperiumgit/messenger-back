using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Start service");
        }
    }
}
