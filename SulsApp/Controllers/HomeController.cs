using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.ViewModels;
using System;
using System.Linq;

namespace SulsApp.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpGet("/")]
        public HttpResponse Index()
        {
            var viewModel = new IndexViewModel
            {
                Message = "Welcome to SULS platform!",
                Year = DateTime.UtcNow.Year
            };
            return this.View(viewModel);
        }
    }
}
