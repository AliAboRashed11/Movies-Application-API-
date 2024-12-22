using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moives.BLL.Dtos;
using Movies.DAL.Data.DbHelper;
using Movies.DAL.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly UserManager<ApplicationUser>_userManager;
        private readonly IConfiguration _config;

        public Account(UserManager<ApplicationUser> userManager,IConfiguration config)
        { 
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RgisterDto UserFromRequest)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                {
                    user.UserName = UserFromRequest.UserName;
                    user.Email = UserFromRequest.Email;
                  
            IdentityResult resul =   await _userManager.CreateAsync(user,UserFromRequest.Password);

                    if (resul.Succeeded)
                    {
                        return Ok("Created");
                    }
                    foreach (var item in resul.Errors) {
                        ModelState.AddModelError("Password",item.Description);
                    
                    }
                };
            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto UserFromRequest)
        {

            if (ModelState.IsValid)
            {

                ApplicationUser userFromDb = await _userManager.FindByNameAsync(UserFromRequest.UserName);
                if (userFromDb != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(userFromDb, UserFromRequest.Password);
                    if (found == true)
                    {
                        List<Claim>userclaim = new List<Claim>();
                        userclaim.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        userclaim.Add(new Claim(ClaimTypes.NameIdentifier,userFromDb.Id));
                        userclaim.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

                        var userRols =await _userManager.GetRolesAsync(userFromDb);
                        foreach (var roleName in userRols) {


                            userclaim.Add(new Claim(ClaimTypes.Role, roleName));
                        }
                        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecritKey"]));

                        SigningCredentials signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);


                        JwtSecurityToken myToken = new JwtSecurityToken(
                           audience: _config["JWT:AudienceIP"],
                           issuer: _config["JWT:IssuerIP"],
                           expires:DateTime.Now.AddHours(1),
                           claims: userclaim,
                           signingCredentials: signingCredentials

                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = myToken.ValidTo
                        });




                    }
                    ModelState.AddModelError("UserName", "UserName Or Password In Valid");
                }
            }
            return BadRequest(ModelState);

        }
    }
}
