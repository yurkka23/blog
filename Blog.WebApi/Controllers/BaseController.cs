namespace Blog.WebApi.Controllers;

[ApiController]
[Route("blog/")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;

    // protected IMediator Mediator { get { return _mediator == null ? HttpContext.RequestServices.GetService<IMediator>() : _mediator ; } }
    protected IMediator Mediator =>
      _mediator ??= HttpContext.RequestServices.GetService<IMediator>();//to form commands when perform request 
    //if mediator null = 
    internal Guid UserId => HttpContext.User.Identity.IsAuthenticated 
        ? Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value) 
        : Guid.Empty;

    internal string UserRole => HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        
}
