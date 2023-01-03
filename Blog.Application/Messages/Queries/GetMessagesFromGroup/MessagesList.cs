namespace Blog.Application.Messages.Queries.GetMessagesFromGroup;

public class MessagesList
{
    public IList<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

}
