namespace InnnoGotchi.DAL.Entities
{
    public class ColoborationRequest
    {
        public int Id { get; set; }
        public int RequestOwnerId { get; set; }
        public User RequestOwner { get; set; }

        public int RequestReceipientId { get; set; }
        public User RequestReceipient { get; set; }

        public DateTime Date { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
