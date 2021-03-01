using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteManager.Business.Users;
using VoteManager.Models.Users;

namespace VoteManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegister request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registerResult = await _service.RegisterUserAsync(request);
            if (registerResult)
                return Ok(new { Message = "User was registered." });

            return BadRequest(new { Message = "User could not be registered." });
        }
    }
}