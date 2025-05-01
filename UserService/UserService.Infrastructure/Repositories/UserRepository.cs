using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Infrastructure.Databases;
using UserService.Domain.Models;
using System.Reflection.Metadata.Ecma335;
using UserService.Application.Contracts.RepositoryContracts;
namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext db;
        public UserRepository(AppDbContext db)
        {
            this.db = db;
        }
        public async Task<bool> CheckByEmail(string email)
        {
            if (await db.Users.AnyAsync(u => u.Email == email))
            {
                return true;
            }
            else return false;
        }
        public async Task<bool> CheckById(string id)
        {
            if (await db.Users.AnyAsync(u => u.Id.ToString().ToLower() == id))
            {
                return true;
            }
            else return false;
        }
        public void AddUser(User user)
        {
            db.Users.Add(user);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            if (await CheckByEmail(email)) return await db.Users.FirstAsync(u => u.Email == email);
            return null;
        }

        public async Task<User> GetUserById(string id)
        {
            if(await CheckById(id)) return await db.Users.FirstAsync(u => u.Id.ToString().ToLower() == id);
            return null;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<User> DeleteUserById(string id)
        {
            User user = await GetUserById(id);

            if(user!=null)db.Users.Remove(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserByEmail(string email)
        {
            if (await CheckByEmail(email))
            {
                User user = await db.Users.FirstAsync(u => u.Email == email);
                return await DeleteUserById(user.Id.ToString().ToLower());
            }
            else return null;
        }
    }
}
