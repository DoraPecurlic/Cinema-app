using Cinema.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Cinema.Service.Common;
using DTO.UserModel;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            _mapper = mapper;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterPost registerPost)
        {
            try
            {
                var user = _mapper.Map<User>(registerPost);
                user.Id = Guid.NewGuid();
                user.DateCreated = DateTime.UtcNow;
                user.DateUpdated = user.DateCreated;
                var registeredUser = await _userService.RegisterUserAsync(user);

                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginPost loginPost)
        {
            try
            {
                var user = _mapper.Map<UserLogin>(loginPost);
                
                var loggedUser = await _userService.LoginUserAsync(user);

                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }
        
        
        [HttpPost("test"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> TestRoute()
        {
            return Ok("radi");
        }
        
    }
}