
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Contracts.TokenContracts;

namespace UserService.Infrastructure.Authentification
{
   public class Hasher : IHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(password,hash);
    }
}
