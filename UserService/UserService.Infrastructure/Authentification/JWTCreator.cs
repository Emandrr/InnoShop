using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using UserService.Application.Contracts.TokenContracts;
using System.Security.Claims;
namespace UserService.Infrastructure.Authentification;
    
public class JWTCreator : IJWTCreator
{
    private readonly JWTOptions _options = new JWTOptions();
   // public JWTCreator(IOptions<JWTOptions> options) => this._options = options.Value;
   
    public string Generate(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString())];
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("71178e3c-00dc-461a-b719-ca95a59b572d")), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpireHours)
            );
        var TokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return TokenValue;
    }
}
