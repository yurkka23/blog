using Blog.Application.Users.Queries.GetProfileUser;

namespace Blog.WebApi.Controllers;

[Route("user/")]
[Authorize]
[ApiController]
public class UserContoller : BaseController
{
    private readonly IMapper _mapper;
    public UserContoller(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }
   

    [HttpGet("get-user-info-by-id")]
    public async Task<ActionResult<UserInfo>> GetUserInfoById(Guid userId,CancellationToken cancellationToken)
    {
        var vm = await Mediator.Send(new GetUserInfoQuery
        {
            Id = userId
        }, cancellationToken);

        return Ok(vm);
    }

    [HttpGet("get-my-info")]
    public async Task<ActionResult<UserInfo>> GetMyInfo( CancellationToken cancellationToken)
    {
        var vm = await Mediator.Send(new GetUserInfoQuery
        {
            Id = UserId
        }, cancellationToken);

        return Ok(vm);
    }
    [HttpGet("get-profile-user")]
    public async Task<ActionResult<ProfileUser>> GetProfileUser(Guid id,CancellationToken cancellationToken)
    {
        var vm = await Mediator.Send(new GetProfileUserQuery
        {
            CurrentUserId = UserId,
            Id = id
        }, cancellationToken);

        return Ok(vm);
    }

    [HttpPut("edit-user-info")]
    public async Task<IActionResult> EditUserInfo([FromBody] EditUserInfoDTO editUserInfoDto, CancellationToken cancellationToken)
    {

        var command = _mapper.Map<EditUserInfoCommand>(editUserInfoDto);
        command.Id = UserId;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("change-role-to-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole([FromBody] UserChangeRoleDTO userChangeRoleDto, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ChangeRoleToAdminCommand>(userChangeRoleDto);
        command.Role = UserRole == "User" ? Role.User : Role.Admin;
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("get-list-of-user")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserList>> GetListUsers(Role role,CancellationToken cancellationToken)
    {
        var query = new GetUsersByRoleQuery
        {
            Role = role
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("search-users-by-username")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserList>> SearchUsersByUsername(string partUsername, CancellationToken cancellationToken)
    {
        var query = new SearchUserQuery
        {
            Role = Role.User,
            PartUsername = partUsername
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("get-statistics")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetStatistics(CancellationToken cancellationToken)
    {
        var query = new GetStatisticsQuery
        {
            Role = UserRole == "User" ? Role.User : Role.Admin
        };
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }

}