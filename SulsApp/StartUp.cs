﻿using Microsoft.EntityFrameworkCore;
using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.Controllers;
using SulsApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp
{
    public class StartUp : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<IProblemsService, ProblemsService>();
            serviceCollection.Add<ISubmissionService, SubmissionService>();
        }

        public void Configure(IList<Route> table)
        {
            var context = new SulsDbContext();
            context.Database.Migrate();
            
        }
    }
}
