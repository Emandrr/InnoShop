using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;
using UserService.Infrastructure.Databases;
using UserService.Infrastructure.Authentification;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Repositories;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using UserService.Application.Contracts.AuthorizationContracts;
using UserService.Application.Contracts.TokenContracts;
using Npgsql.Internal;
namespace UserService.Application.Services
{
   public class AuthService : IAuthService
    {
        private readonly IJWTCreator jwtCreator= new JWTCreator();
        private readonly IHasher Hasher = new Hasher();
        private readonly AppDbContext db;
        private readonly UserRepository us;
        public AuthService(AppDbContext db)
        {
            
            this.db = db;
            us = new UserRepository(db);
        }

        public async Task<List<string>> Login(string userName,string email,string password)
        {
            bool answer = await us.CheckByEmail(email);
            if (!answer) return null;

            User user = await us.GetUserByEmail(email);
            var result = Hasher.Verify(password,user.PasswordHash);
            if (result == false) return null;
            var token = jwtCreator.Generate(user);

            List<string> user_info = new List<string>();
            user_info.Add(token);
            user_info.Add(user.Id.ToString());
            return user_info;
            
        }
        public async Task<User> Register(string userName,string email, string password)
        {
            bool answer = await us.CheckByEmail(email);
            if (answer) return null;
            if (!answer)
            {
                var hash = Hasher.Hash(password);
                
                var usr = User.Create(Guid.NewGuid(), userName, email, hash, DateTime.Now.ToUniversalTime());
                string tokenValue = jwtCreator.Generate(usr);
               // SmtpService smtpService = new SmtpService(HttpContext.);
                
                await db.Users.AddAsync(usr);
                await db.SaveChangesAsync();
                return usr;
            }
            return null;
        }
    }
}
