using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using SulsApp.Models;
using SulsApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService userService;
        private ILogger logger;
        public UsersController(IUsersService usersService, ILogger logger)
        {
            this.userService = usersService;
            this.logger = logger;
        }
        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            string username = this.Request.FormData["username"];
            string password = this.Request.FormData["password"];

            var userId = this.userService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);
            this.logger.Log("User logged in: " + username);
            return this.Redirect("/");
        }


        public HttpResponse Register()
        {
            return this.View();

        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            var username = this.Request.FormData["username"];
            var email = this.Request.FormData["email"];
            var password = this.Request.FormData["password"];
            var confirmPassword = this.Request.FormData["confirmPassword"];

            if (password != confirmPassword)
            {
                return this.Error("Passwords doesn`t match!");
            }

            if (username?.Length < 5 || username?.Length > 20)
            {
                return this.Error("Username should be in range 5 - 20 characters.");
            }

            if (!IsValid(email))
            {
                return this.Error("Invalid email address!");
            }


            if (password?.Length < 6 || password?.Length > 20)
            {
                return this.Error("Password should be in range 6 - 20 characters.");
            }

            this.userService.CreateUser(username, email, password);
            this.logger.Log("New User: " + username);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }
        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
