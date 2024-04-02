using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopApi.Dtos;
using ShopApi.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly ShopContext _context;

        public AuthController(IAuthService authService, IConfiguration config, ShopContext context)
        {
            _authService = authService;
            _config = config;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await _authService.UserExists(userForRegisterDto.UserName))
                return BadRequest("Username already exists");

            //var userToCreate = _mapper.Map<User>(userForRegisterDto);


            var role = _context.Roles.FirstOrDefault(x=>x.Id == userForRegisterDto.RoleId);

            if (role == null)
            {
                return BadRequest("This role does not exist");
            }


            var userRolePair = new UsersRole()
            {
                RoleID = role.Id
            };

            List<UsersRole> userRolePairs = new List<UsersRole>();
            userRolePairs.Add(userRolePair);

            User  userToCreate = new()
            {
                
                UserName = userForRegisterDto.UserName,
                Email = userForRegisterDto.Email,
                Created = DateTime.Now,
                UsersRoles = userRolePairs
            };

            var createdUser = await _authService.Register(userToCreate, userForRegisterDto.Password);

            //var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);


            //return CreatedAtRoute("GetUser", new
            //{
            //    controller = "Users",
            //    id = createdUser.Id
            //}, createdUser);
            return Ok();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _authService.Login(userForLoginDto.UserName
                .ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.Now.AddDays(1), 
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                userFromRepo
            });
        }

        [Authorize]
        [HttpGet("GetUserById")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }


    }
}

