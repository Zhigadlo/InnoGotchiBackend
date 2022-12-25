using InnnoGotchi.DAL.Entities;
using System.Text.Json.Serialization;

namespace InnoGotchi.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[]? Avatar { get; set; }

        [JsonIgnore]
        public List<ColoborationRequest> SentRequests { get; set; }

        [JsonIgnore]
        public List<ColoborationRequest> ReceivedRequests { get; set; }

        [JsonIgnore]
        public List<ColoborationRequestDTO> ColoborationRequests { get; set; } = new List<ColoborationRequestDTO>();

        public FarmDTO? Farm { get; set; }
    }
}
