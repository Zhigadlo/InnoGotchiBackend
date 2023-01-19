using System.Text.Json.Serialization;

namespace InnoGotchi.BLL.DTO
{
    public class FarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public List<PetDTO> Pets { get; set; } = new List<PetDTO>();

        public int OwnerId { get; set; }

        [JsonIgnore]
        public UserDTO Owner { get; set; }
    }
}
