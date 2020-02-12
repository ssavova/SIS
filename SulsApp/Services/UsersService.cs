using SIS.MvcFramework;
using SulsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SulsApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly SulsDbContext db; 
        public UsersService(SulsDbContext db)
        {
            this.db = db;
        }
        public void ChangePassword(string username, string newPassword)
        {
            var user = this.db.Users.FirstOrDefault(u => u.Username == username);
            if(user == null)
            {
                return;
            }

            user.Password = this.Hash(newPassword);
            this.db.SaveChanges();
        }

        public int CountUsers()
        {
            return this.db.Users.Count();
        }

        public void CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = this.Hash(password),
                Role = IdentityRole.User,
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = this.Hash(password);
            return this.db.Users.Where(u => u.Username == username && u.Password == passwordHash)
                .Select(u => u.Id).FirstOrDefault();
        }

        public bool IsEmailUsed(string email)
        {
           return this.db.Users.Any(u => u.Email == email);
        }

        public bool IsUsernameUsed(string username)
        {
            return this.db.Users.Any(u => u.Username == username);
        }

        private string Hash(string input)
        {
            var crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
