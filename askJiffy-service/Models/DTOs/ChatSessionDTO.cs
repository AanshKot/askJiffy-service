namespace askJiffy_service.Models.DTOs
{
    public class ChatSessionDTO
    {
        public int Id { get; set; }
        public required UserDTO User { get; set; }
        public required VehicleDTO Vehicle { get; set; }
        // https://stackoverflow.com/questions/10113244/why-use-icollection-and-not-ienumerable-or-listt-on-many-many-one-many-relatio
        public required ICollection<ChatMessageDTO> ChatMessages { get; set; }
        //when create chat session endpoint recieves a message/question, the title is created in the business logic, streamed back to component
        public required string Title { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
