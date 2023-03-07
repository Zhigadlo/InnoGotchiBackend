using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace InnoGotchi.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettings = configuration.GetSection(nameof(TokenSettings));
            var key = tokenSettings.GetValue<string>(nameof(TokenSettings.Key));
            var issuer = tokenSettings.GetValue<string>(nameof(TokenSettings.Issuer));
            var audience = tokenSettings.GetValue<string>(nameof(TokenSettings.Audience));
            var lifetime = tokenSettings.GetValue<int>(nameof(TokenSettings.ExpireTimeHours));
            var authOptions = new AuthOptions(issuer, audience, lifetime, key);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });
        }
    }
}
