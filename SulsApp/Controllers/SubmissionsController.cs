using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.Models;
using SulsApp.Services;
using SulsApp.ViewModels.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SulsApp.Controllers
{
    public class SubmissionsController : Controller
    {
        
        private readonly ISubmissionService submissionService;
        private readonly SulsDbContext db;
        public SubmissionsController(SulsDbContext db, ISubmissionService submissionServcie)
        {
            this.db = db;
            this.submissionService = submissionServcie;
        }
        
        public HttpResponse Create(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var problem = this.db.Problems
               .Where(x => x.Id == id).FirstOrDefault();

            if (problem == null)
            {
                return this.Error("Problem not found!");
            }

            var viewModel = new CreateFormViewModel()
            {
                Name = problem.Name,
                ProblemId = problem.Id,
            };
                
            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(string problemId,string code)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(code == null && code.Length < 30)
            {
                return this.Error("Code must be at least 30 characters");
            }



            this.submissionService.CreateSubmission(this.User, problemId, code);

            return Redirect("/");
        }
        
        
        public HttpResponse Delete(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.submissionService.Delete(id);
            return this.Redirect("/");
        }
        
    }
}
