using Cafe.DTOs;
using Cafe.Helpers;
using Cafe.Implementations;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;

        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            configuration = _configuration;
            unitOfWork = _unitOfWork;
        }


        [HttpPost("register")] //api/account/register
        //create dto for register data (  عشان مش استخدم identityuser  واجبره يدخل كل البيانات اللي حتي مش محتاحجها)
        public async Task<IActionResult> Register( [FromBody] RegisterUserDTO userDTO)
        {

            if (ModelState.IsValid)
            {
                var userInSite = unitOfWork.AppUser.GetAll(u => u.Email == userDTO.Email, null, "").FirstOrDefault();

                if (userInSite != null)
                {
                    return BadRequest("Not Allowed");
                }

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                };
                IdentityResult res = await userManager.CreateAsync(user, userDTO.Password);
                //asign role to user
                userManager.AddToRoleAsync(user, WebSiteRoles.SiteUser).GetAwaiter().GetResult();

                if (res.Succeeded)
                {
                    return Ok( new { message = "account created" } );
                }
                else
                {
                    //return BadRequest(res.Errors);

                    //forloop to show all errors
                    foreach (var item in res.Errors)
                    {
                        return BadRequest(item.Description);
                    }

                }
            }

            return BadRequest(ModelState);

        }



        [HttpPost("login")] //api/account/login
        public async Task<IActionResult> LogIn([FromBody] LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                //check username
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {
                    //check pass
                    bool res = await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (res)
                    {
                        //(2)
                        var Allclaims = new List<Claim>();
                        Allclaims.Add(new Claim(ClaimTypes.Name, user.UserName)); //custom claim


                        if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteAdmin))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin));
                        }
                        else if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteDelivery))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteDelivery));
                        }
                        else if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteUser))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteUser));
                        }
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin)); //custom claim
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteDoctor)); //custom claim
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SitePatient)); //custom claim



                        Allclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); //custom claim
                        Allclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //predifne claims ==> token id

                        //(3)
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secretKey"]));
                        SigningCredentials signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //create token (1)
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: configuration["JWT:issuer"], // web api server url
                            audience: configuration["JWT:audiance"], //angular url
                            claims: Allclaims,
                            expires: DateTime.Now.AddDays(2),
                            signingCredentials: signingCredential
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = myToken.ValidTo
                        }
                            );
                    }
                    else
                    {
                        return Unauthorized("Password Wrong");

                    }
                }
                else
                {
                    return Unauthorized("User Name not Found");
                }

            }
            else
            {
                //return Unauthorized();
                return BadRequest(ModelState);
            }
        }




    }
}
