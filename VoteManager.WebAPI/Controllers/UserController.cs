using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteManager.Business.Tokens;
using VoteManager.Business.Users;
using VoteManager.Models.Tokens;
using VoteManager.Models.Users;

namespace VoteManager.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registerResult = await _userService.RegisterUserAsync(request);
            if (registerResult)
                return Ok(new { Message = "User was registered." });

            return BadRequest(new { Message = "User could not be registered." });
        }

        [AllowAnonymous]
        [HttpPost("~/api/Token")]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tokenResponse = await _tokenService.GetTokenAsync(request);
            if (tokenResponse is null)
                return BadRequest(new { Message = "Invalid username or password." });

            return Ok(tokenResponse);
        }

        [HttpPut("{userId}/Deactivate")]
        public async Task<IActionResult> DeactivateAccount([FromRoute] int userId)
        {
            if (userId.ToString() != (User.Identity as ClaimsIdentity).FindFirst("Id")?.Value && !User.IsInRole("Admin"))
                return Unauthorized();

            var deactivationResponse = await _userService.DeactivateUserAsync(userId);
            if (deactivationResponse)
                return Ok(new { Message = $"User {userId} deactivated." });

            return BadRequest(new { Message = "Could not deactivate the targeted user." });
        }
    }
}