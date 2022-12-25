using System.ComponentModel.DataAnnotations.Schema;

namespace InnnoGotchi.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public byte[]? Avatar { get; set; }

        public List<ColoborationRequest> SentRequests { get; set; }
        public List<ColoborationRequest> ReceivedRequests { get; set; }
        public List<Farm> CollaboratedFarms { get; set; }

        public Farm? Farm { get; set; }
    }
}
