﻿using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.MvcFramework;
using SulsApp.Services;
using SulsApp.ViewModels;
using SulsApp.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SulsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly SulsDbContext db;
        public HomeController(ILogger logger, SulsDbContext db)
        {
            this.logger = logger;
            this.db = db;
        }
        
        [HttpGet("/")]
        public HttpResponse Index()
        
        {
            if(this.IsUserLoggedIn())
            {
                var problems = this.db.Problems.Select(x => new IndexProblemViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.Submissions.Count()
                }).ToList();

                var loggedInviewModel = new LoggedInViewModel()
                {
                    Problems = problems
                };

                return this.View(loggedInviewModel,"IndexLoggedIn");
            }

            this.logger.Log("Hello from Index");
            var viewModel = new IndexViewModel
            {
                Message = "Welcome to the Stefi`s Simple App",
                Year = DateTime.UtcNow.Year,
            };
            return this.View(viewModel);
            
        }
    }
}
