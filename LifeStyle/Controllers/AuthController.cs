using LifeStyle.Application.Abstractions;
using LifeStyle.Application.Auth;
using LifeStyle.Application.Services;
using LifeStyle.Application.Users.Responses;
using LifeStyle.Domain.Models.Users;
using LifeStyle.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LifeStyle.ConsolePresentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LifeStyleContext _lifeStyleContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;



        public AuthController(LifeStyleContext lifeStyleContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IdentityService identityService,
            IUnitOfWork unitOfWork)
        {
            _lifeStyleContext = lifeStyleContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register registerDto)
        {


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
                UserName = registerDto.Username,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var newClaims = new List<Claim>
             {
                 new("Email", registerDto.Email)
             };

            await _userManager.AddClaimsAsync(user, newClaims);

            if (registerDto.Role == RoleEnum.Admin)
            {
                var role = await _roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    role = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(role);

                }
                await _userManager.AddToRoleAsync(user, "Admin");

                newClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            else
            {
                var role = await _roleManager.FindByNameAsync("User");
                if (role == null)
                {
                    role = new IdentityRole("User");
                    await _roleManager.CreateAsync(role);
                }
                await _userManager.AddToRoleAsync(user, "User");
                newClaims.Add(new Claim(ClaimTypes.Role, "User"));

            }


            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub,user.Email ?? throw new InvalidOperationException()),
                new(JwtRegisteredClaimNames.Email,user.Email ?? throw new InvalidOperationException()),
            });

            claimsIdentity.AddClaims(newClaims);

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthResult(_identityService.WriteToken(token));

            return Ok(response);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login loginDto)
        {
            
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
            var userRoles = await _userManager.GetRolesAsync(user);

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub,user.Email ?? throw new InvalidOperationException()),
                new(JwtRegisteredClaimNames.Email,user.Email ?? throw new InvalidOperationException()),
            });
            claimsIdentity.AddClaims(userClaims);

            foreach (var role in userRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthResult(_identityService.WriteToken(token));

            return Ok(response);
        }


        [HttpGet("admin")]
        [Authorize]
        public IActionResult GetAdminData()
        {
            return Ok("This is protected data for Admins only.");
        }


        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUserData()
        {
            return Ok("This is protected data for Users only.");
        }


        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileDto updateUserProfileDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User not logged in.");
            }

            var userProfile = await _unitOfWork.UserProfileRepository.GetByName(email);
            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            userProfile.PhoneNumber = updateUserProfileDto.PhoneNumber;
            userProfile.Height = updateUserProfileDto.Height;
            userProfile.Weight = updateUserProfileDto.Weight;
            userProfile.PhotoUrl = updateUserProfileDto.PhotoUrl;
            userProfile.BirthDate = updateUserProfileDto.BirthDate;
            userProfile.Gender = updateUserProfileDto.Gender;

            await _unitOfWork.UserProfileRepository.Update(userProfile);
            await _unitOfWork.SaveAsync();

            return Ok("Profile updated successfully.");
        }
    }
}

