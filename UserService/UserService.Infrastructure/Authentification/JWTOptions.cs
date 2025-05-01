using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Authentification
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = "1";
        
        public int ExpireHours { get; set; }
    }
}
