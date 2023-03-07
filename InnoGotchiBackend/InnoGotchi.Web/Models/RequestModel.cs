namespace InnoGotchi.Web.Models
{
    /// <summary>
    /// Request model that contains data from view
    /// </summary>
    public class RequestModel
    {
        public int RequestOwnerId { get; set; }
        public int RequestReceipientId { get; set; }
    }
}
