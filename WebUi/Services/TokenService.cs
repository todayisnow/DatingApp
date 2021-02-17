using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebUi.Entities;
using WebUi.Interfaces;

namespace WebUi.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["tokenKey"]));
        }



        public string CreateToken(AppUser user) =>
             new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
             {
                 Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)

            }),
                 Expires = DateTime.Now.AddDays(7),
                 SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
             }));


        //var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.NameId,User.UserName)

        //    };
        //var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        //var tokenDesc = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(claims),
        //    Expires = DateTime.Now.AddDays(7),
        //    SigningCredentials = creds
        //};
        //var tokenHandler = new JwtSecurityTokenHandler();
        //var token = tokenHandler.CreateToken(tokenDesc);
        //    return tokenHandler.WriteToken(token);



    }
}
