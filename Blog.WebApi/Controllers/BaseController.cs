namespace Blog.WebApi.Controllers;

[ApiController]
[Route("blog/")]
public abstract class BaseController : ControllerBase
{
    protected IMediator Mediator;

    public BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    internal Guid UserId => HttpContext.User.Identity.IsAuthenticated 
        ? Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value) 
        : Guid.Empty;

    internal string UserRole => HttpContext.User.Identity.IsAuthenticated
        ? HttpContext.User.FindFirst(ClaimTypes.Role).Value
        : Role.User.ToString();

    internal string UserName => HttpContext.User.Identity.IsAuthenticated
        ? HttpContext.User.FindFirst(ClaimTypes.Name).Value
        : "";

}
