using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;
namespace UserService.Application.Contracts.AuthorizationContracts
{
    public interface IAuthService
    {
        public Task<User> Register(string userName, string email, string password);
        public Task<List<string>> Login(string userName, string email, string password);
    }
}
