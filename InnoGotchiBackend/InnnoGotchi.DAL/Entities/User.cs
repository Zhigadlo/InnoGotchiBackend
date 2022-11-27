namespace InnnoGotchi.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<User> Friends { get; set; }
        public List<Farm> CollaboratedFarms { get; set; }
        public string? AvatarURL { get; set; }
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
