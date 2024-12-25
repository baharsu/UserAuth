using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserAuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            var response = await _authenticationService.RegisterUser(registerDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            var response = await _authenticationService.LoginUser(loginDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("Verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyDto verifyDto)
        {
            var response = await _authenticationService.VerifyUser(verifyDto.username, verifyDto.verificationCode, verifyDto.loginStartTime);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var response = await _authenticationService.ForgotPassword(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string code, string newPassword)
        {
            var response = await _authenticationService.ResetPassword(code, newPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOutUser(string username)
        {
            var response = await _authenticationService.LogOutUser(username);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
