
namespace Blog.Application.Users.Queries.GetUsersByRole;

public class UserList
{
    public IList<UserLookUpDto> Users { get; set; } = new List<UserLookUpDto>();
}
