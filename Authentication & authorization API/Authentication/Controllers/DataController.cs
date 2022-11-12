using Authentication.DATA.Models;
using Authentication.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DataController : ControllerBase
    {
        
        private readonly UserManager<USERS> _userManager;
        private readonly IConfiguration _configuration;


        public DataController(UserManager<USERS> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        #region  Register

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterDto registerDTO)
        {
            var newReg = new USERS()
            {
                SchoolName = registerDTO.SchoolName,
                UserName = registerDTO.UserName
            };

            var creationResult = await _userManager.CreateAsync(newReg, registerDTO.Password);
            if (!creationResult.Succeeded)
            {
                return BadRequest(creationResult.Errors);
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newReg.Id),
            new Claim(ClaimTypes.Role, "Student"),
        };

            var clainmsResult = await _userManager.AddClaimsAsync(newReg, claims);
            if (!clainmsResult.Succeeded)
            {
                return BadRequest(clainmsResult.Errors);
            }

            return Ok();
        }
        #endregion

        #region Login


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto loginDTO)
        {
            var Obj = await _userManager.FindByNameAsync(loginDTO.UserName);

            if (Obj is null)
            {
                return BadRequest();
            }

            if (!await _userManager.CheckPasswordAsync(Obj, loginDTO.Password))
            {
                
                return Unauthorized();
            }

            var claim1 = await _userManager.GetClaimsAsync(Obj);

            var keyString = _configuration.GetValue<string>("SecretKey");
            var keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);

            var signingCredentials =
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                claims: claim1,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("tokenDuration")),
                notBefore: DateTime.Now
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);

            return Ok(new
            {
                Token = tokenString,
                Expiry = DateTime.Now.AddMinutes(_configuration.GetValue<int>("tokenDuration"))
            });
        }
        #endregion

        #region Check Authorization




        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
        {
            return Ok(new List<string> { "Hi", "Hiiiiii" });
        }


        [HttpGet]
        [Route("Authorization")]
        [Authorize(policy:"Student")]
        public ActionResult CheckAuthority()
        {
            return Ok(new List<string> { "Hi", "Hiiiiii" });
        }

        [HttpGet]
        [Route("Authentication")]
        [Authorize(policy:"Teacher")]
        public ActionResult checkAuthorization()
        {
            return Ok("Welcome Teacher");
        }
    }
}
#endregion