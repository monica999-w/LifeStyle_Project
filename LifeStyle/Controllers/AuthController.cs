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
        private readonly IFileService _fileService;
        private readonly ILogger<AuthController> _logger;



        public AuthController(LifeStyleContext lifeStyleContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IdentityService identityService,
            IUnitOfWork unitOfWork,
            IFileService fileService,
            ILogger<AuthController> logger)
        {
            _lifeStyleContext = lifeStyleContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _logger = logger;
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

            UserProfile userProfile = new()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserId = user.Id
            };
            await _lifeStyleContext.UserProfiles.AddAsync(userProfile);
            await _lifeStyleContext.SaveChangesAsync();

            var newClaims = new List<Claim>
    {
        new("Email", registerDto.Email)
    };

            await _userManager.AddClaimsAsync(user, newClaims);

            // Set the role based on the email
            if (registerDto.Email == "admin@admin.com")
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
        new(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
        new(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException())
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


        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfileDto)
        {
            try
            {
                _logger.LogInformation("Updating user profile with data: {@UpdateUserProfileDto}", updateUserProfileDto);

                var email = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("User not found with ID: {Email}", email);
                    return Unauthorized("User not logged in.");
                }

                var userProfile = await _unitOfWork.UserProfileRepository.GetByName(email);

                if (userProfile == null)
                {
                    _logger.LogWarning("User profile not found for user with: {Email}", email);
                    return NotFound("User profile not found.");
                }

                if (updateUserProfileDto.PhoneNumber != null)
                    userProfile.PhoneNumber = updateUserProfileDto.PhoneNumber;

                if (updateUserProfileDto.Height.HasValue)
                    userProfile.Height = updateUserProfileDto.Height.Value;

                if (updateUserProfileDto.Weight.HasValue)
                {
                    if (userProfile.Weight != updateUserProfileDto.Weight.Value)
                    {
                        var weightEntry = new WeightHistory
                        {
                            Weight = updateUserProfileDto.Weight.Value,
                            Date = DateTime.UtcNow,
                            UserProfileId = userProfile.ProfileId
                        };
                        userProfile.WeightEntries.Add(weightEntry);
                    }

                    userProfile.Weight = updateUserProfileDto.Weight.Value;
                }

                if (updateUserProfileDto.BirthDate.HasValue)
                    userProfile.BirthDate = updateUserProfileDto.BirthDate.Value;

                if (updateUserProfileDto.Gender != null)
                    userProfile.Gender = updateUserProfileDto.Gender;

                if (updateUserProfileDto.PhotoUrl != null)
                    userProfile.PhotoUrl = await _fileService.SaveFileAsync(updateUserProfileDto.PhotoUrl, "uploads");

                _lifeStyleContext.UserProfiles.Update(userProfile);
                await _lifeStyleContext.SaveChangesAsync();

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user profile.");
                return StatusCode(500, "Internal server error.");
            }
        }




        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                    return Unauthorized("User not logged in.");
                }

                var userProfile = await _unitOfWork.UserProfileRepository.GetByName(email);
                if (userProfile == null)
                {
                    _logger.LogWarning("User profile not found for user with email: {Email}", email);
                    return NotFound("User profile not found.");
                }

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user profile.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("profile/weightHistory")]
        [Authorize]
        public async Task<IActionResult> GetWeightHistory()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                    return Unauthorized("User not logged in.");
                }

                var userProfile = await _lifeStyleContext.UserProfiles
                    .Include(u => u.WeightEntries)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (userProfile == null)
                {
                    _logger.LogWarning("User profile not found for user with email: {Email}", email);
                    return NotFound("User profile not found.");
                }

                var weightHistory = userProfile.WeightEntries
                    .OrderByDescending(e => e.Date)
                    .Select(e => new WeightEntryDto
                    {
                        Id = e.Id,
                        Weight = e.Weight,
                        Date = e.Date
                    })
                    .ToList();

                return Ok(weightHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the weight history.");
                return StatusCode(500, "Internal server error.");
            }
        }

     }
}

