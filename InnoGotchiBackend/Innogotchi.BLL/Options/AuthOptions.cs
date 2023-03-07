using InnoGotchi.BLL.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnoGotchi.BLL.Options
{
    /// <summary>
    /// Contains options for jwt token
    /// </summary>
    public class AuthOptions
    {
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public int Lifetime { get; private set; }

        private string _key;

        public AuthOptions(IOptions<TokenSettings> settings)
        {
            var tokenSettings = settings.Value;
            _key = tokenSettings.Key;
            Issuer = tokenSettings.Issuer;
            Audience = tokenSettings.Audience;
            Lifetime = tokenSettings.ExpireTimeHours;
        }
        public AuthOptions(string issuer, string audience, int lifetime, string key)
        {
            Issuer = issuer;
            Audience = audience;
            Lifetime = lifetime;
            _key = key;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
