using System.ComponentModel.DataAnnotations;

namespace JobFinder.Core.Models.AuthViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
