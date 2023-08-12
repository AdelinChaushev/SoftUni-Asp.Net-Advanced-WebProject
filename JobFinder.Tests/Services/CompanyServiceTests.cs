using JobFinder.Core.Contracts;
using JobFinder.Core.Services;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Tests
{
    public class Tests
    {
        private  List<Company> companies;
        private  ICompanyServiceInterface companyService;
       
        private  JobFinderDbContext context;

        private string userId1 = "1";
        private string userId2 = "2";
        private string userId3 = "3";
        private string userId4 = "4";

        private string  spaceXId = "b7795c10-296b-407d-9645-3048eae835ff";
        private string appleId = "0ae89d35-00d0-4a43-95af-c154c6eeb37e";
        private string nikeId = "ae7af307-ffe6-4720-a2c1-ccf89cfff46c";

        private Guid jobCategoryId = Guid.NewGuid();
        private Guid jobSchedule = Guid.NewGuid();
        private Guid jobListingGuid = Guid.NewGuid();

        private Guid pictureId = Guid.NewGuid();
        private string picturePath = "Path";




        [SetUp]
        public  void Setup()
        {
            this.companies = new List<Company>() {
            new (){ Id = Guid.Parse(spaceXId), CompanyName = "SpaceX", CompanyDescription = "SpaceX is an inovative company that studies the space" ,OwnerId = userId1},
            new (){ Id = Guid.Parse(appleId), CompanyName = "Apple", CompanyDescription = "Apple is one of the largest companies in the word that is know for making high end smartphones,laptops....etc." ,OwnerId = userId2},
            new (){ Id = Guid.Parse(nikeId), CompanyName = "Nike", CompanyDescription = "Nike is one the leading brands in the cloting industry" , OwnerId = userId3 }
            };
            Interview interview = new Interview()
            {
                CompanyId = Guid.Parse(appleId),
                UserId = userId4,
                JobTitle = "Enginner",
                InterviewStart = DateTime.Now.AddDays(2),
                InterviewEnd = DateTime.Now.AddDays(2).AddHours(2),
            };
            ApplicationUser user = new()
            {
                Id = userId4,
                Email = "adelin@gamil.com",
                UserName = "Adelin",
                EmailConfirmed = true,
                NormalizedUserName = "ADELIN",


            };
            JobCategory jobCategory = new()
            {
                Id = jobCategoryId,
                Name = "Enginnering"
            };
            Schedule schedule = new()
            {
                Id = jobSchedule,
                WorkingSchedule = "9-5"
            };

            JobListing jobListing = new()
            {
                Id = jobListingGuid,
                CompanyId = Guid.Parse(appleId),
                Company = companies[1],
                JobCategoryId = jobCategoryId,
                ScheduleId = jobSchedule,
                JobTitle = "Enginner",
                Description = "Working with the design and architecture of MAC,Iphone ..... etc",
                SalaryPerMonth = 12000,
                VaccantionDays = 20,
                

            };
            Picture picture = new()
            {
                Id = pictureId,
                PicturePath = picturePath,
                CompanyId = Guid.Parse(spaceXId)

            };
            
            var options = new DbContextOptionsBuilder<JobFinderDbContext>()
            .UseInMemoryDatabase(databaseName: "JobFinderDbContext") // Use an in-memory DB
            .Options;

            this.context = new JobFinderDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            this.context.AddRange(this.companies); // Add data to the DB
            this.context.Add(user);
            this.context.Add(jobCategory);
            this.context.Add(schedule);
            this.context.Add(interview);
            this.context.Add(jobListing);
            this.context.Add(picture);
            this.context.SaveChanges();

            companyService = new CompanyService(context);
        }


        [Test]
        public async Task Test_Company_GetCompany_By_Id()
        {
           Company company = await companyService.GetCompanyById(Guid.Parse(spaceXId));

            Assert.That(company.CompanyDescription == "SpaceX is an inovative company that studies the space");
            Assert.That(company.CompanyName == "SpaceX");
            Assert.That(company.Id == Guid.Parse(spaceXId));
        }

        [Test]
        public async Task Test_Company_GetCompany_By_UserId()
        {
            Company company = await companyService.GetCompanyByUserId(userId1);

            Assert.That(company.CompanyDescription == "SpaceX is an inovative company that studies the space");
            Assert.That(company.CompanyName == "SpaceX");
            Assert.That(company.Id == Guid.Parse(spaceXId));
        }

        [Test]

        public async Task Test_Company_Edit()
        {
           
            var editedCompany = new Company()
            {
                CompanyName = "SpaceX",
                CompanyDescription = "SpaceX is an inovative company that studies the space!!!!"
            };
            await companyService.EditAsync(editedCompany, userId1);
            Assert.That(companies[0].CompanyDescription == editedCompany.CompanyDescription);
            Assert.That(companies[0].CompanyName == editedCompany.CompanyName);
        }
        [Test]

        public async Task Test_Company_Add()
        {
            Company company = new Company()
            {
                CompanyName = "Bmw",
                CompanyDescription = "BMW is one the leading make manifactureer in the world"
            };
            await companyService.AddAsync(company, userId4);
            Company companyAdded = await companyService.GetCompanyByUserId(userId4);
            Assert.That(companyAdded.CompanyDescription == company.CompanyDescription);
            Assert.That(companyAdded.CompanyName == company.CompanyName);
        }
        [Test]
        public async Task Test_Company_Delete()
        {
            await companyService.DeleteAsync(userId2);

            var result = await companyService.GetCompanyByUserId(userId2);
            Assert.That(result, Is.Null);
        }
        [Test]

        public async Task Test_Company_Company_Interviews()
        {
          var interviews =   await companyService.GetCompanyInterviewsAsync(userId2);
            Assert.That(interviews.Count() == 1);
            Assert.That(interviews.First().CompanyId == Guid.Parse(appleId));
            Assert.That(interviews.First().UserId == userId4);
        }
        [Test]
        public async Task Test_Company_SearchForCompanies()
        {
            var companies = await companyService.SearchForCompanies("a");
            Assert.That(companies.Count() == 2);
            Assert.That(companies.Any(c => c.Id == Guid.Parse(spaceXId)));
            Assert.That(companies.Any(c => c.Id == Guid.Parse(appleId)));
        }

        [Test]
        public async Task Test_Company_CompanyJobListings()
        {
            var jobLisitingsByUserId = await companyService.GetAllByJobListingsAsync(userId2);
            var jobLisitingsById = await companyService.GetAllByJobListingsAsync(Guid.Parse(appleId));
            Assert.That(jobLisitingsByUserId.Count() == 1);
            Assert.That(jobLisitingsByUserId.Any(c => c.Id == jobListingGuid));
            Assert.That(jobLisitingsById.Count() == 1);
            Assert.That(jobLisitingsById.Any(c => c.Id == jobListingGuid));

        }
        [Test]
        public async Task Test_Company_GetCompanyPictures()
        {
            var picturesByUserId = await companyService.GetCompanyPictures(userId1);

            Assert.That(picturesByUserId.Count() == 1);
            Assert.That(picturesByUserId.Any(c => c.Base64 == $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(picturePath))}"));
            Assert.That(picturesByUserId.Any(c => c.Id == pictureId));

            var picturesById = await companyService.GetCompanyPictures(Guid.Parse(spaceXId));

            Assert.That(picturesById.Count() == 1);
            Assert.That(picturesById.Any(c => c.Base64 == $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(picturePath))}"));
            Assert.That(picturesByUserId.Any(c => c.Id == pictureId));

        }
        [Test]
        public async Task Test_Company_DeleteById_Positive()
        {
            await companyService.DeleteAsyncById(Guid.Parse(spaceXId));
            Assert.That(context.Companies.Count() == 2);
            Assert.That(!await context.Companies.AnyAsync(c => c.Id == Guid.Parse(spaceXId)));
        }
        [Test]
        public async Task Test_Company_DeleteById_Negative()
        {
          Assert.ThrowsAsync<InvalidOperationException>(async () =>  await companyService.DeleteAsyncById(Guid.NewGuid()));
        }   


    }
}