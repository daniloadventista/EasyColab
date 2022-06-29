using System;

namespace API.Entities
{
    public class Sample
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
    }
}