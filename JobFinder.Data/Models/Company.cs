using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(35)]
        [MinLength(3)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(25)]
        [MaxLength(800)]
        public string CompanyDescription { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<JobListing> JobListings { get; set; }

        public List<Interview> Interviews { get; set; }

        

        public List<Picture> Pictures { get; set; }
    }
}
