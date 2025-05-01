using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.CustomExceptions
{
    public class UserNotFoundException(string message) :Exception(message);
}
