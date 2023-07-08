using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Models
{
    public class Resume
    {
        public Guid Id { get; set; }
        public string ResumePath { get; set; }

        [ForeignKey(nameof(ApplicationUser))]

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
