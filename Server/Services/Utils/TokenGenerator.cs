using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PKiK.Server.DB;
using PKiK.Server.Shared;
using System;
using System.Security.Claims;
using System.Text;

namespace PKiK.Server.Services.Utils
{
    internal static class TokenGenerator
    {
        internal static string UserToken(UserDBO user)
        {
            var config = Config.Get();

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan iat = DateTime.UtcNow - origin;
            int intIAT = (int)Math.Floor(iat.TotalSeconds);

            TimeSpan exp = DateTime.UtcNow.AddDays(config.JWT.DaysValid) - origin;
            int intEXP = (int)Math.Floor(exp.TotalSeconds);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, intIAT.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, intEXP.ToString())
            };
            var jwtToken = new JwtSecurityToken(config.JWT.Issuer,
                config.JWT.Audience ?? "", claims,
                signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JWT.SecretKey)),
                        SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
