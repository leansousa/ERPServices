using ERPServices.Identity.Domain;
using ERPServices.Identity.Helper;
using ERPServices.Identity.Model;
using ERPServices.Identity.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERPServices.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtAuthHelper _jwtAuthHelper;
        public AuthController(IUserRepository userRepository, JwtAuthHelper jwtAuthHelper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtAuthHelper = jwtAuthHelper ?? throw new ArgumentNullException(nameof(jwtAuthHelper));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Auth([FromBody] UserModel user)
        {
            try
            {
                var userExists = _userRepository.GetByEmail(user.Email ?? "");

                if (userExists == null)
                    return BadRequest(new { Message = "Invalid Data" });


                if (userExists?.Password != user.Password)
                    return BadRequest(new { Message = "Invalid Data" });


                var token = _jwtAuthHelper.Generate(userExists ?? new UserEntity());

                return Ok(token);

            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Generic Error" });
            }
        }
    }
}
