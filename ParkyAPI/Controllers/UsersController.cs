using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        [AllowAnonymous]//When people goes here, he has not been authenticated yet
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _userRepository.Authenticate(model.UserName!, model.Password!);
            if (user == null)
                //return BadRequest(ModelState);
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User model)
        {
            bool ifUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepository.Register(model.UserName, model.Password);
            if (user == null) return BadRequest(new { message = "Error while registering" });
            return Ok();
        }

    }
}
