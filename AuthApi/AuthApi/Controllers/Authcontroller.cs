using AuthApi.Models;
using AuthApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authcontroller(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupRequest request)
        {
            if (request == null) return BadRequest("Email daxil edin");

            if (appDbContext.Users.Any(u => u.Email == request.Email)) return BadRequest("Bu email sistemde var artiq.");
            var user = new User { Email = request.Email, Password = request.Password };
            appDbContext.Users.Add(user);
            appDbContext.SaveChanges();

            return Ok($"User qeydiyyat edildi id: {user.Id}");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest loginRequest)
        {
            if (!appDbContext.Users.Any(u => u.Email == loginRequest.Email))
                return BadRequest("Bele email yoxdu sistemda qeydiyyat edin");

            var user = appDbContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
            if (user.Password == loginRequest.Password)
                return Ok("Giris edildi");

            return BadRequest("Sifre yanlisdi");
        }
    }
}
