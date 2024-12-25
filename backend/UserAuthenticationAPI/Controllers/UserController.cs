using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationAPI.Services.UserService;

namespace UserAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetUserByToken")]
        public async Task<IActionResult> GetUserByToken()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Token is missing or invalid.");
                }
                var token = authorizationHeader.Replace("Bearer ", string.Empty);

                var response = await _userService.GetUserByToken(token);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }

                return Ok(new
                {
                    response.Data.Name,
                    response.Data.Surname,
                    response.Data.Email,
                    response.Data.Username,
                    response.Data.IsOnline
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserRole")]
        public async Task<IActionResult> GetUserRole()
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Token is missing or invalid.");
                }
                var token = authorizationHeader.Replace("Bearer ", string.Empty);
                var response = await _userService.GetUserRole(token);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
