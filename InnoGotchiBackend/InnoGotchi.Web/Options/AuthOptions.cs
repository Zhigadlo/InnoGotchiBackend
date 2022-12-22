using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnoGotchi.Web.Options
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthIssuer";
        public const string AUDIENCE = "AuthClient";
        const string KEY = "mymegasecret_secretkeyAAAAAAAAAA";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
