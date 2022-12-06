using InnoGotchi.BLL.DTO;

namespace InnoGotchi.Web.Models
{
    public class PetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppearanceDTO Appearance { get; set; }
        public int FarmId { get; set; }
    }
}
