using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.TokenContracts
{
    public interface IHasher
    {
        public string Hash(string password);

        public bool Verify(string key,string hashedpassword);
    }
}
