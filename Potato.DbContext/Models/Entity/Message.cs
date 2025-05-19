namespace Potato.DbContext.Models.Entity
{
    public class Message
    {
        public Message()
        {
            Timestamp =  DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string ? Text { get; set; }

        public string ? SenderId { get; set; }
        public User? Sender { get; set; }

        public string? RecipientId { get; set; }
        public User? Recipient { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}
