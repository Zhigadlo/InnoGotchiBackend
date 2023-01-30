using InnoGotchi.BLL.Models;
using System.Security.Claims;

namespace InnoGotchi.Web.Models
{
    public class IdentityTokenModel
    {
        public ClaimsIdentity Identity { get; set; }
        public SecurityTokenModel Token { get; set; }
    }
}
