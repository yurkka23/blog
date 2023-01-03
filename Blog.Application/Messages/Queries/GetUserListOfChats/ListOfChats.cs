

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class ListOfChats
{
    public IList<ChatDTO> Chats { get; set; } = new List<ChatDTO>();
}
