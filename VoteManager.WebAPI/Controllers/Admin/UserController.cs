using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteManager.Business.Users;
using VoteManager.Models.Users;

namespace VoteManager.WebAPI.Controllers_Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] AdminUserRegister request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registerResult = await _userService.RegisterUserAsync(request, request.Role);
            if (registerResult)
                return Ok(new { Message = "User was registered." });

            return BadRequest(new { Message = "User could not be registered." });
        }
    }
}