using System.ComponentModel.DataAnnotations;

namespace JobFinder.Core.Models.CompanyViewModels
{
    public class CompanyInputViewModel
    {
        [Required]
        [MaxLength(35)]
        [MinLength(3)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(25)]
        [MaxLength(800)]
        public string CompanyDescription { get; set; }
        
    }
}
