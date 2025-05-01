using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Application.Contracts.RepositoryContracts
{
    public interface IUserRepository
    {
        public Task<bool> CheckByEmail(string email);
        public Task<bool> CheckById(string id);
        public void AddUser(User user);

        public Task<User> GetUserByEmail(string email);
        public Task<User> GetUserById(string id);
        public Task<List<User>> GetAllUsers();
        public Task<User> DeleteUserById(string id);
        public Task<User> DeleteUserByEmail(string email);
    }
}
