using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AutentificacionJWT
{
    public class Startup
    {
        public IConfiguration ConfigRoot { get; }

        public Startup(IConfiguration configuration)
        {
            ConfigRoot = configuration;
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var appSettingHelper = new AppSettings(ConfigRoot);

            var jwtSettings = appSettingHelper.GetJwtAppSetting();

            if (jwtSettings != null)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret))
                    };
                });
            }
        }

        private void AddInjetion(IServiceCollection services)
        {
            services.AddScoped(provider => new AppSettings(ConfigRoot));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AddAuthentication(services);
            AddInjetion(services);
        }

        public void Configure(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            app.Run();
        }
    }
}
