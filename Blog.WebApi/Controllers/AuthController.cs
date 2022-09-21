using System.Linq;

namespace Blog.WebApi.Controllers;

[Route("auth/")]
[ApiController]

public class AuthController : BaseController
{
   
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IBlogDbContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService, IBlogDbContext blogContext, IMapper mapper,
        SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userService = userService;
        _blogContext = blogContext;
        _mapper = mapper;
        _signInManager = signInManager;
        _userManager = userManager;
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
            return BadRequest("User already exists");
        }

        User newUser = new User
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
            await _signInManager.SignInAsync(newUser, false);//false - if we close browser, cookie delete 
            return Ok(newUser.Id);
        }

        return BadRequest(result.Errors);
    }
    [AllowAnonymous]
    [HttpPost("login/")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDTO request)
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
        var result = _signInManager.CanSignInAsync(user);

        if (!res.Result.Succeeded)
        {
            return BadRequest("Wrong password");
        }

       
        var token = _userService.CreateToken(user , _configuration);
       
        var refreshToken = _userService.GenerateRefreshToken();
        await _userService.SetRefreshToken(refreshToken, user, HttpContext, _blogContext, CancellationToken.None);
        
        return Ok(token);
    }

    [Authorize]
    [HttpGet("logout/")]
    public async Task<IActionResult> Logout()//redo it!!!
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        //var myid = ClaimTypes.NameIdentifier.ToString();
        Guid UserId1 = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier.ToString()).Value);

    var user = await _blogContext.Users.FirstOrDefaultAsync(u => u.Id == UserId1);//with UserId don't find : null
       

        var refreshToken = Request.Cookies["refreshToken"];
        if(user == null)
        {
            return NotFound("Not found such user");
        }

        if (!user.RefreshToken.Equals(refreshToken))
        {
            return Unauthorized("Invalid Refresh Token.");
        }
        if (user.TokenExpires < DateTime.Now)
        {
            return Unauthorized("Token expired.");
        }

        string token = _userService.CreateToken(user, _configuration);
        var newRefreshToken = _userService.GenerateRefreshToken();
        
        await _userService.SetRefreshToken(newRefreshToken,user,HttpContext, _blogContext, CancellationToken.None);

        return Ok(token);
    }

}