using AutoMapper;
using LifeStyle.Application.Auth;
using LifeStyle.Application.Services;
using LifeStyle.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace LifeStyle.ConsolePresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityService _identityService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IdentityService identityService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityService = identityService;
           
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            IdentityUser user = new()
            {
                Email = registerDto.Email,
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = registerDto.PhoneNumber,
                UserName =registerDto.Username,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleName = registerDto.IsAdmin ? "Admin" : "User";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            var newClaims = new List<Claim>
        {
            new(ClaimTypes.Email, registerDto.Email)
           
        };

            await _userManager.AddClaimsAsync(user, newClaims);

            var claimsIdentity = new ClaimsIdentity(newClaims);
            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthResultDto(_identityService.WriteToken(token));

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                Console.WriteLine($"User not found: {loginDto.Email}");
                return BadRequest("Invalid login attempt. User not found.");
            }

            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                Console.WriteLine($"Invalid password for user: {loginDto.Email}");
                return BadRequest("Invalid login attempt. Incorrect password.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var claimsIdentity = new ClaimsIdentity(userClaims);

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthResultDto(_identityService.WriteToken(token));

            return Ok(response);
        }
    }
}