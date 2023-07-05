using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Model
{
    /// <summary>
    /// The review class s the feedback that users have about a company
    /// </summary>
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Range(1,5)]
        public int ReviewScore { get; set; }
        [MaxLength(35)]
        [MinLength(5)]
        public string ReviewSumary { get; set; } = null!;
        [MaxLength(400)]
        public string? AdditionalComment { get; set; }

        [ForeignKey(nameof(Company))]

        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
