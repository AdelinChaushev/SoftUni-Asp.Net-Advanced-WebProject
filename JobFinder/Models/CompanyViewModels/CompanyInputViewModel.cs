using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models.CompanyViewModels
{
    public class CompanyInputViewModel
    {
        [Required]
        [MaxLength(35)]
        [MinLength(3)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(80)]
        [MaxLength(800)]
        public string CompanyDescription { get; set; }
        
    }
}
