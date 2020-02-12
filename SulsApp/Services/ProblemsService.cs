using SulsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly SulsDbContext context;
        public ProblemsService(SulsDbContext context)
        {
            this.context = context;
        }
        public void CreateProblem(string name, int points)
        {
            Problem problem = new Problem()
            {
                Name = name,
                Points = points,
            };

            this.context.Problems.Add(problem);
            this.context.SaveChanges();
        }
    }
}
