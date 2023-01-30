using InnoGotchi.BLL.Models;
using System.Security.Claims;

namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// Contains ClaimsIdentity and SecurityTokenModel
    /// </summary>
    public class IdentityTokenModel
    {
        public ClaimsIdentity Identity { get; set; }
        public SecurityTokenModel Token { get; set; }
    }
}
