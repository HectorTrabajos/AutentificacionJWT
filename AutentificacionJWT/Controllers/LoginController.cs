using AutentificacionJWT.Helper;
using AutentificacionJWT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutentificacionJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppSettings appSettings;

        public LoginController(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Email != "Admin" || user.Password != "1234")
            {
                return BadRequest();
            }

            var jwt = appSettings.GetJwtAppSetting();

            var token = JwtHelper.Crear(
                jwt.AccessTokenSecret,
                jwt.Issuer,
                jwt.Audience,
                jwt.AccessTokenExpirationMinutes,
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, "HECTOR CHUMPITAZ"),
                    new Claim(ClaimTypes.Role, "ADMINISTRADOR")
                }
            );

            var refreshToken = JwtHelper.Crear(
                jwt.RefreshTokenSecret,
                jwt.Issuer,
                jwt.Audience,
                jwt.RefreshTokenExpirationMinutes
            );

            return Ok(new { Token = token, RefreshToken = refreshToken });

        }
    }
}
