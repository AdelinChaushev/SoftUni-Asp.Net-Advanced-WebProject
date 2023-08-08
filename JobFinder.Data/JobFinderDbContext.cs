using JobFinder.Data.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Data
{
    public class JobFinderDbContext : IdentityDbContext<ApplicationUser>
    {
        public JobFinderDbContext(DbContextOptions<JobFinderDbContext> options)
            : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Interview> Interviews { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<JobCategory> JobCategories { get; set; }

        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Resume> Resumes { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Interview>().HasKey(k => new { k.CompanyId, k.UserId });
            builder.Entity<Interview>()
                .HasOne(k => k.User)
                .WithMany(k => k.Interviews)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //   .Entity<JobCategory>()
            //   .HasData(new JobCategory()
            //   {
            //       Id = Guid.NewGuid(),
            //       Name = "Tech"
            //   },
            //   new JobCategory()
            //   {
            //       Id = Guid.NewGuid(),
            //       Name = "Farming"
            //   },
            //   new JobCategory()
            //   {
            //       Id = Guid.NewGuid(),
            //       Name = "Finnance"
            //   },
            //   new JobCategory()
            //   {
            //       Id = Guid.NewGuid(),
            //       Name = "Architecture"
            //   });

            //builder
            // .Entity<Schedule>()
            // .HasData(new Schedule()
            // {
            //     Id = Guid.NewGuid(),
            //     WorkingSchedule = "9-5"
            // },
            // new Schedule()
            // {
            //     Id = Guid.NewGuid(),
            //     WorkingSchedule = "Weekends"
            // },
            // new Schedule()
            // {
            //     Id = Guid.NewGuid(),
            //     WorkingSchedule = "4 hours a day"
            // },
            // new Schedule()
            // {
            //     Id = Guid.NewGuid(),
            //     WorkingSchedule = "full working week"
            // });

            builder.Entity<JobApplication>().HasKey(k => new { k.JobListingId, k.UserId });

            builder.Entity<JobApplication>()
                .HasOne(k => k.JobListing)
                .WithMany(k => k.UsersApplications)
                .OnDelete(DeleteBehavior.Restrict);

         builder.Entity<JobApplication>()
         .HasOne(k => k.User)
         .WithMany(k => k.JobApplications)
         .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Resume>()
                .HasOne(c => c.User)
                .WithOne(c => c.Resume)
                .HasForeignKey<Resume>(c => c.UserId);
           



            base.OnModelCreating(builder);
        }
    }
}
