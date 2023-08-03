using JobFinder.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserServiceInterface userService;

        public UserController(IUserServiceInterface userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Interviews()
        {
            var models = await userService.GetInterviewsAsync(GetUserId());
            return View(models);
        }


    }
}
