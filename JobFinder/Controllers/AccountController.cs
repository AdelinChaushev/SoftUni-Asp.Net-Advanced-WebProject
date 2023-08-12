using JobFinder.Data.Models;
using JobFinder.Core.Models.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Contracts;

namespace JobFinder.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserServiceInterface userService;
     

       

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserServiceInterface userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
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
                return BadRequest();
            }
            return View();
        }
        
        public async Task<IActionResult> CreateEmployerAccount()
        {
            if (User.IsInRole("Employer"))
            {
                return BadRequest();
            }
            ApplicationUser user = await userManager.FindByIdAsync(GetUserId());
            if (!await userService.UserHasCompany(GetUserId()))
            {
                return BadRequest();
            }
            await userManager.AddToRoleAsync(user, "Employer");
            await signInManager.SignInAsync(user, isPersistent: true);

            return Redirect("/Employer/Home/Index");



        }
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteEmployerAccount()
        {
            ApplicationUser user = await userManager.FindByIdAsync(GetUserId());
            await userManager.RemoveFromRoleAsync(user, "Employer");
            await signInManager.SignInAsync(user, isPersistent: true);
            return RedirectToAction("Index", "Home");


        }
        public IActionResult AccountSettings()
        {
            return View();
        }
        public async Task<IActionResult> DeleteAccount()
        {
            ApplicationUser user = await userManager.FindByIdAsync(GetUserId());

            await signInManager.SignOutAsync();
            await userService.DeleteInterviewsAndJoblistings(GetUserId());
            await userManager.DeleteAsync(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
