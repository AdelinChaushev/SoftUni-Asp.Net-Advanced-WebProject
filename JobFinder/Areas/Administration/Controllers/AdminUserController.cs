using JobFinder.Core.Contracts;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminUserController : AdminBaseController
    {
        private readonly IUserServiceInterface userService;

        private readonly UserManager<ApplicationUser> userManager;

        public AdminUserController(IUserServiceInterface userService
            , UserManager<ApplicationUser> userManager)
        {


            this.userService = userService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> SearchForUser(string keyword)
        {
            var usersViewModel = await userService.SearchForUser(keyword);
            return View(usersViewModel);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (await userManager.IsInRoleAsync(user,"Admin"))
            {
                ModelState.AddModelError("", "Can not delete admins.");
                return RedirectToAction(nameof(SearchForUser));
            }
            
            await userManager.RemoveFromRolesAsync(user, new List<string>() { "Employer", "User" });
            await userService.DeleteInterviewsAndJoblistings(id);
            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(SearchForUser));
        }
    }
}
