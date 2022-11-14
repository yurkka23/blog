namespace Blog.WebApi.Controllers;

[Route("auth/")]
[ApiController]

public class AuthController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IBlogDbContext _blogContext;
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;

    public AuthController(IUserService userService, IBlogDbContext blogContext,
        SignInManager<User> signInManager, UserManager<User> userManager, IMediator mediator, ICacheService cacheService) : base(mediator)
    {
        _userService = userService;
        _blogContext = blogContext;
        _signInManager = signInManager;
        _userManager = userManager;
        _cacheService = cacheService;   
    }
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register([FromBody] UserRegisterDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var query = new CheckUserQuery
        {
            UserName = request.UserName
        };
        var existUser = await Mediator.Send(query);

        if (existUser)
        {
            return BadRequest("User with such username already exists");
        }

        var newUser = new User
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            AboutMe = request.AboutMe,
            Role = Role.User
        };
        
        var result = await _userManager.CreateAsync(newUser, request.Password);


        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(newUser, false);
            await _cacheService.DeleteAsync("UserListSearch");
            return Ok(newUser.Id);
        }

        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login/")]
    public async Task<ActionResult<AuthRefreshDTO>> Login([FromBody] UserLoginDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _blogContext.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);


        if (user == null)
        {
            return BadRequest("User with such username doesn't exist");
        }

        var res = _signInManager.PasswordSignInAsync(request.UserName, request.Password,false,false);

        if (!res.Result.Succeeded)
        {
            return BadRequest("Wrong password");
        }

       
        var token = _userService.CreateToken(user);
       
        var refreshToken = _userService.GenerateRefreshToken();
        await _userService.SetRefreshToken(refreshToken, user, HttpContext, _blogContext, CancellationToken.None);
        var response = new AuthResponseDTO
        {
            JwtToken = token,
            RefreshToken = refreshToken.Token,
            User = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe,
                ImageUserUrl = user.ImageUserUrl,
                Role = user.Role
            }
        };
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthRefreshDTO>> RefreshToken([FromBody] AuthRequestDTO request)
    {
        var user = await _blogContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
       
        if(user == null)
        {
            return NotFound("Not found such user");
        }

        if (!user.RefreshToken.Equals(request.RefreshToken))
        {
            return BadRequest("Invalid Refresh Token.");
        }
        if (user.TokenExpires < DateTime.UtcNow)
        {
            return BadRequest("Token expired.");
        }

        string token = _userService.CreateToken(user);
        var newRefreshToken = _userService.GenerateRefreshToken();
        
        await _userService.SetRefreshToken(newRefreshToken,user,HttpContext, _blogContext, CancellationToken.None);

        var response = new AuthRefreshDTO
        {
            jwtToken = token,
            refreshToken = newRefreshToken.Token
        };
        return Ok(response);
    }

}