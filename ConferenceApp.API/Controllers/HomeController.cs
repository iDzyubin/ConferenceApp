using Microsoft.AspNetCore.Mvc;

namespace ConferenceApp.API.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => Ok( "ConferenceApp API." );
    }
}