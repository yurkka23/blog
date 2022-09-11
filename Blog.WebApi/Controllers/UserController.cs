using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Blog.Application.Users.Queries.GetUserInfo;
using Blog.WebApi.DTOs.UserDTOs;
using Blog.Application.Users.Commands.EditUserInfo;

namespace Blog.WebApi.Controllers;

[Route("user/")]
[ApiController]
public class UserContoller : BaseController
{
    private readonly IMapper _mapper;
    public UserContoller(IMapper mapper) => _mapper = mapper;

    [HttpGet("GetUserInfoById")]
    public async Task<ActionResult<UserInfoVm>> GetUserInfoById(Guid userId)
    {
        var vm = await Mediator.Send(new GetUserInfoQuery
        {
            Id = userId
        });

        return Ok(vm);
    }

    [HttpPut("EditUserInfo")]
    public async Task<IActionResult> EditUserInfo([FromBody] EditUserInfoDTO editUserInfoDto)
    {
        var command = _mapper.Map<EditUserInfoCommand>(editUserInfoDto);
        command.Id = UserId;
        await Mediator.Send(command);
        return NoContent();
    }
}