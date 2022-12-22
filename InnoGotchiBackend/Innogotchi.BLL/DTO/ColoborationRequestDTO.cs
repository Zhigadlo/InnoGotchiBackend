using InnnoGotchi.DAL.Entities;
using System.Text.Json.Serialization;

namespace InnoGotchi.BLL.DTO
{
    public class ColoborationRequestDTO
    {
        public int Id { get; set; }
        public int RequestOwnerId { get; set; }
        [JsonIgnore]
        public User RequestOwner { get; set; }

        public int RequestReceipientId { get; set; }
        [JsonIgnore]
        public User RequestReceipient { get; set; }

        public DateTime Date { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
