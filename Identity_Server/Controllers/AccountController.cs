using Microsoft.AspNetCore.Mvc;
using Identity_Server.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Numerics;

namespace Identity_Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            var user = new IdentityUser
            {
                UserName = loginVm.UserName
            };
            
            await _userManager.CreateAsync(user, loginVm.Password);
    
            var result = await _signInManager
                    .PasswordSignInAsync(loginVm.UserName, loginVm.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(loginVm.ReturnUrl);
            }
                
            return View();
        }
       
    }
}
