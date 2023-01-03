

namespace Blog.Application.Messages.Queries.GetUserListOfChats;

public class ChatDTO
{
    public Guid RecipientUserId { get; set; }
    public string RecipientUsername { get; set; }
    public string RecipientAvatarUrl { get; set; }

}
