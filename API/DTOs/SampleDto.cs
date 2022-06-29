using System;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class SampleDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Product { get; set; }
        public string ProductId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        
        [JsonIgnore]
        public bool SenderDeleted { get; set; }

        
    }
}