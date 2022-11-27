namespace InnoGotchi.BLL.DTO
{
    public class FarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PetDTO> Pets { get; set; }
        public List<UserDTO> Сollaborators { get; set; }
        public int OwnerId { get; set; }
    }
}
