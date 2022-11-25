namespace InnoGotchi.BLL.DTO
{
    public class FarmDTO
    {
        public List<PetDTO> Pets { get; set; }
        public List<UserDTO> Сollaborators { get; set; }
        public int OwnerId { get; set; }
        public UserDTO Owner { get; set; }
    }
}
