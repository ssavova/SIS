using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.MvcFramework;
using SulsApp.Services;
using SulsApp.ViewModels;
using SulsApp.ViewModels.Home;
using System;


namespace SulsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }
        
        [HttpGet("/")]
        public HttpResponse Index()
        {
            this.logger.Log("Hello from Index Action");
            var viewModel = new IndexViewModel
            {
                Message = "Welcome to SULS platform!",
                Year = DateTime.UtcNow.Year
            };
            return this.View(viewModel);
        }
    }
}
