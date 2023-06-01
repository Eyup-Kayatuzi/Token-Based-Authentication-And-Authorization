using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WepApiCrudWithJwt.DTOs;
using WepApiCrudWithJwt.Identity;
using WepApiCrudWithJwt.Models;
using WepApiCrudWithJwt.Options;
using WepApiCrudWithJwt.Services;

namespace WepApiCrudWithJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public AuthController(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet("CurrentUser")]
        [Authorize]
        public IActionResult GetMyName([FromServices] ICurrentUser currentUser)
        {
            /*var pathBase = HttpContext.Request.PathBase;
            var targetClaim = User.FindFirst(ClaimTypes.Role);
            return Ok(targetClaim.Value);*/
            return Ok(currentUser.CurrentUserName());
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpAppUserDto registeredUser, string role = "USER")
        {
            var userExistByEmail = await _userManager.FindByEmailAsync(registeredUser.Email);
            var userExistByUsername = await _userManager.FindByNameAsync(registeredUser.Username);
            if (userExistByEmail != null || userExistByUsername != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel { Status = "Error", Message = "Bu kullanıcı sistemde mevcuttur!" });
            }
            AppIdentityUser user = new()
            {
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                Email = registeredUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registeredUser.Username,
                EmailConfirmed = true
            };
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registeredUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return StatusCode(StatusCodes.Status201Created, new ResponseModel { Status = "Success", Message = "Kullanıcı oluşturuldu!" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Failed", Message = "Kullanıcı oluşturulurken hata oluştu!" });
                }

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Failed", Message = "Böyle bir rol bulunmamaktadır!" });
            }

        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromServices] IOptions<JwtOption> jwtOption, SignInAppUserDto signedInUser)
        {
            var user = await _userManager.FindByNameAsync(signedInUser.Username);
            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    if (await _userManager.CheckPasswordAsync(user, signedInUser.Password))
                    {
                        
                        var authClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Email, user.Email), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())/*, new Claim(JwtRegisteredClaimNames.Email, user.Email)*/ }; // her iki email durumunda da çalıştı
                        var userRoles = await _userManager.GetRolesAsync(user);
                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }
                        var signIn = await _signInManager.PasswordSignInAsync(user, signedInUser.Password, false, false);
                        if (signIn.Succeeded)
                        {
                            var jwtToken = GetToken2(jwtOption, authClaims, 1, 1);
                        var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                            return Ok(new { Token = tokenStr, expiration = jwtToken.ValidTo.ToLocalTime() });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                            {
                                Status = "Giriş başarısız!",
                                Message = "Sistemde bir hata meydana geldi, hemen ilgileniyoruz!"
                            });

                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel()
                        {
                            Status = "Giriş başarısız!",
                            Message = "Girilen şifre hatalıdır!"
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel()
                    {
                        Status = "Giriş başarısız!",
                        Message = "Email onayı olmadan giriş yapamazsınız!"
                    });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel()
                {
                    Status = "Sistemde böyle bir kullanıcı adı bulunamadı!",
                    Message = "Girilen kullanıcı adı hatalıdır!"
                });
            }
        }

        private JwtSecurityToken GetToken2(IOptions<JwtOption> jwtOption, List<Claim> authClaims, int hours, int minutes) // senin fazladan eklediğin kısım 
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Value.Secret));
            var utcNow = DateTime.Now;
            var expirationTime = utcNow.AddHours(hours).AddMinutes(minutes);
            var token = new JwtSecurityToken(
                issuer: jwtOption.Value.ValidIssuer,
                audience: jwtOption.Value.ValidAudience,
                expires: expirationTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
