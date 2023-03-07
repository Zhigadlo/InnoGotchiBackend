namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// Pet model that contains data from view
    /// </summary>
    public class PetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Appearance { get; set; }
        public int FarmId { get; set; }
    }
}
