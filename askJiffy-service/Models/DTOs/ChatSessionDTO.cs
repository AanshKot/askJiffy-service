namespace askJiffy_service.Models.DTOs
{
    public class ChatSessionDTO
    {
        public int Id { get; set; }
        public required string SubjectId { get; set; }
        public required string VehicleId { get; set; }
        // https://stackoverflow.com/questions/10113244/why-use-icollection-and-not-ienumerable-or-listt-on-many-many-one-many-relatio
        public ICollection<ChatMessageDTO>? ChatMessages { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
