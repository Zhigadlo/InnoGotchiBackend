using Microsoft.Extensions.Configuration;
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

        public AuthOptions(IConfiguration configuration)
        {
            _key = configuration.GetSection("TokenKey").Value;
            Issuer = configuration.GetSection("TokenIssuer").Value;
            Audience = configuration.GetSection("TokenAudience").Value;
            Lifetime = int.Parse(configuration.GetSection("TokenExpireTimeHours").Value);
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
