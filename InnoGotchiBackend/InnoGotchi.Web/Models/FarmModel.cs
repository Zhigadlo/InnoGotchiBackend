namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// Farm model that contains data from view
    /// </summary>
    public class FarmModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
    }
}
