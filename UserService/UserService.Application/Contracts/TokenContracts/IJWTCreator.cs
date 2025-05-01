using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Application.Contracts.TokenContracts
{
    public interface IJWTCreator
    {
        public string Generate(User user);
    }
}
