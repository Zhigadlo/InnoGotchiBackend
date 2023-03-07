namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// User model that contains data from view
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Avatar { get; set; }
    }
}
