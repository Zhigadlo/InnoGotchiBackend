namespace InnoGotchi.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? AvatarURL { get; set; }

        public List<ColoborationRequestDTO> ColoborationRequests { get; set; }
        public List<FarmDTO> CollaboratedFarms { get; set; }

        public int FarmId { get; set; }
        public FarmDTO Farm { get; set; }
    }
}
