using Blog.Application.Users.Queries.CheckUser;

namespace Blog.WebApi.Controllers;

[Route("user/")]
[ApiController]
public class UserContoller : BaseController
{
    private readonly IMapper _mapper;
    public UserContoller(IMapper mapper) => _mapper = mapper;

    [HttpGet("GetUserInfoById")]
    [Authorize]
    public async Task<ActionResult<UserInfoVm>> GetUserInfoById(Guid userId)
    {
        var vm = await Mediator.Send(new GetUserInfoQuery
        {
            Id = userId
        });

        return Ok(vm);
    }

    [HttpGet("GetMyInfo")]
    [Authorize]
    public async Task<ActionResult<UserInfoVm>> GetMyInfo()
    {
        var vm = await Mediator.Send(new GetUserInfoQuery
        {
            Id = UserId
        });

        return Ok(vm);
    }

    [HttpPut("EditUserInfo")]
    [Authorize]
    public async Task<IActionResult> EditUserInfo([FromBody] EditUserInfoDTO editUserInfoDto)
    {
        var query = new CheckUserQuery
        {
            UserName = editUserInfoDto.UserName
        };
        var existUser = await Mediator.Send(query);

        if (existUser)
        {
            return BadRequest("User already exists");
        }

        var command = _mapper.Map<EditUserInfoCommand>(editUserInfoDto);
        command.Id = UserId;
        await Mediator.Send(command);
        return NoContent();
    }
}