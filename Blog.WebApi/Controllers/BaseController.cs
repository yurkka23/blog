using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blog.WebApi.Controllers;

[ApiController]
[Route("blog/")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();//to form commands when perform request 

    //internal Guid UserId => !User.Identity.IsAuthenticated
    //    ? Guid.Empty
    //    : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


    internal Guid UserId => HttpContext.User.Identity.IsAuthenticated 
        ? Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value) 
        : Guid.Empty;
    

}
