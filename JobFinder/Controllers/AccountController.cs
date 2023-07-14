using JobFinder.Data.Models;
using JobFinder.Core.Models.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
     

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        => View();

        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> Register(UserRegisterViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }
            ApplicationUser user =  new ApplicationUser()
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email,
            };

         var result =  await userManager.CreateAsync(user,userViewModel.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user,isPersistent:true);
               await userManager.AddToRoleAsync(user,"User");
                return RedirectToAction(nameof(SelectAccountType));
                
            }
          
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(userViewModel);
            
        }

        [AllowAnonymous]
        [HttpGet]

        public IActionResult Login(string? returnURL)
        => View();


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel loginViewModel, string? returnURL)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            ApplicationUser user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {

                var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false);

                if (result.Succeeded)
                {
                    if (returnURL != null)
                    {
                        return Redirect(returnURL);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
               ModelState.AddModelError("", "Invalid Login");
               return View(loginViewModel);

          
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SelectAccountType()
        {
            if( await userManager.IsInRoleAsync(await userManager.FindByIdAsync(GetUserId()), "Employer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        public async Task<IActionResult> CreateEmployerAccount()
        {
            ApplicationUser user = await userManager.FindByIdAsync(GetUserId()); 
            await userManager.AddToRoleAsync(user, "Employer");

            return Redirect("/Employer/Home/Index");


        }
        public IActionResult AccountSettings()
        {
            return View();
        }

    }
}
