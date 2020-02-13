using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using SulsApp.Models;
using SulsApp.Services;
using SulsApp.ViewModels.Users;
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
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
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

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords doesn`t match!");
            }

            if (input.Username?.Length < 5 || input.Username?.Length > 20)
            {
                return this.Error("Username should be in range 5 - 20 characters.");
            }

            if (!IsValid(input.Email))
            {
                return this.Error("Invalid email address!");
            }


            if (input.Password?.Length < 6 || input.Password?.Length > 20)
            {
                return this.Error("Password should be in range 6 - 20 characters.");
            }

            if(this.userService.IsUsernameUsed(input.Username))
            {
                return this.Error("This username is already used!");
            }

            if (this.userService.IsEmailUsed(input.Email))
            {
                return this.Error("This email is already used!");
            }

            this.userService.CreateUser(input.Username, input.Email, input.Password);
            this.logger.Log("New User: " + input.Username);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

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
