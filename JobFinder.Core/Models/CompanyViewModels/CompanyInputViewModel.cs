using System.ComponentModel.DataAnnotations;
using static JobFinder.Common.DataValidationConstants.Company;

namespace JobFinder.Core.Models.CompanyViewModels
{
    public class CompanyInputViewModel
    {
        [Required]
        [MaxLength(NameMaxLenght)]
        [MinLength(NameMinLenght)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(DescriptionMinLenght)]
        [MaxLength(DescriptionMaxLenght)]
        public string CompanyDescription { get; set; }
        
    }
}
