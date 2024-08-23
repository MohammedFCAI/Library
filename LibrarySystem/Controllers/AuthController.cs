using LibrarySystem.Dtos;
using LibrarySystem.Models;
using LibrarySystem.Models.Authentications;
using LibrarySystem.Services.Authentications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.AddToRoleAsync(user, "NORMALUSER");
            return Ok("Registration successful");
        }


        [HttpPost("register-v2")]
        public async Task<IActionResult> Register2(Register model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            await _userManager.AddToRoleAsync(user, "NORMALUSER");
            return Ok("Registration successful");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid login attempt");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (!user.IsApproved)
            {
                return BadRequest("Not approved yet!");
            }
            return Ok("Login successful");
        }



        [HttpPost("login-v2")]
        public async Task<IActionResult> Login2(Login model)
        {
            var userResponse = await _userService.LoginWithJwt(model);
            if (!userResponse.Success)
                return NotFound(userResponse.Message);
            return Ok(new
            {
                tokens = userResponse.Entities
            });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(UserRole role)
        {
            var user = await _userManager.FindByNameAsync(role.UserName);
            if (user == null)
            {
                return BadRequest("Invalid username");
            }
            else
            {
                var result = role.Role.ToUpper();

                var userHasRole = await _userManager.GetRolesAsync(user);
                if (!userHasRole.IsNullOrEmpty())
                {
                    var userRoleName = userHasRole[0].ToUpper();
                    if (userRoleName == result)
                        return BadRequest("User has already this role!");
                }
                try
                {
                    await _userManager.AddToRoleAsync(user, result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok("Successfully assigned");
        }

        [HttpPost("approve-user")]
        public async Task<IActionResult> ApproveUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("Invalid Username");
            }
            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
            return Ok("Approved");
        }
    }

}
