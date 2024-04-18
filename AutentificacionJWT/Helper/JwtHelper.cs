using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutentificacionJWT.Helper
{
    public class JwtHelper
    {
        public static string Crear(string key, string issuer, string audience, double expirity, IEnumerable<Claim>? claims = null)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new(issuer, audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expirity), credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
