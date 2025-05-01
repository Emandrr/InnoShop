using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Records
{
    public record ChangeUser(string id,string UserName, string email, string Password);
}
