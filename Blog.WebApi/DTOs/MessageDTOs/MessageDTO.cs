namespace Blog.WebApi.DTOs.MessageDTOs;

public class MessageDTO
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderPhotoUrl { get; set; }

    public Guid RecipienId { get; set; }
    public string RecipienUsername { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
   
}
