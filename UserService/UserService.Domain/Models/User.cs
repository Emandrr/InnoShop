using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace UserService.Domain.Models
{
    public class User 
    {
        public User(Guid Id,string Name, string Email,string PasswordHash,DateTime RefreshTokenExpireTime)
        {
            this.Email = Email;
            this.RefreshTokenExpireTime = RefreshTokenExpireTime.ToUniversalTime();
            this.PasswordHash = PasswordHash;
            this.Id = Id;
            this.Name = Name;
        }
        public string PasswordHash { get; private set; }
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public DateTime RefreshTokenExpireTime { get;private set; } 
        public static User Create(Guid Id, string userName, string Email, string PasswordHash, DateTime RTimeExpire)
        {
            return new User(Id, userName,Email, PasswordHash, RTimeExpire.ToUniversalTime());
        }

    }
}
