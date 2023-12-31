﻿using JobFinder.Core.Contracts;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobListingViewModels;

namespace JobFinder.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class CompanyController : EmployerBaseController
    { 
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            Company compamy = await companyService.GetCompanyByUserId(GetUserId());
            CompanyInputViewModel companyOutputViewModel = new CompanyInputViewModel() 
            { 
                CompanyName = compamy.CompanyName,
                CompanyDescription = compamy.CompanyDescription
            };


           return View(companyOutputViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CompanyInputViewModel compnayViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid input");
                return View(compnayViewModel);
            }
            var company = ToDbModel(compnayViewModel);
         
           await companyService.EditAsync(company, GetUserId());
            
           
            return RedirectToAction("CompanySettings");
        }
       
        [HttpGet]
        public async Task<IActionResult> CompanyInterviews()
        {
          var model = await companyService.GetCompanyInterviewsAsync(GetUserId());
           return View(model);
        }
        public async Task<IActionResult> Delete()
        {
        
            await companyService.DeleteAsync(GetUserId());
            return Redirect("/Account/DeleteEmployerAccount");
        }
        public async Task<IActionResult> CompanySettings()
        {
            Company companyDbModel = await companyService.GetCompanyByUserId(GetUserId());
            var companyViewModel = new CompanySettingViewModel()
            {
                CompanyName = companyDbModel.CompanyName,
                CompanyDescription = companyDbModel.CompanyDescription,
            };
            return View(companyViewModel);
        }

        public async Task<IActionResult> CompanyPictures()
        {
            var pictures = await companyService.GetCompanyPictures(GetUserId());
            return View(pictures);
        }

         
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
         => new ()
            {
                CompanyDescription = compnayViewModel.CompanyDescription,
                CompanyName = compnayViewModel.CompanyName,
            };

        private IEnumerable<JobListingOutputViewModel> ToViewModelJobListings(IEnumerable<JobListing> dbCollection)
        => dbCollection.Select(c => new JobListingOutputViewModel()
        {
            Id = c.Id,
            JobTitle = c.JobTitle,
            Description = c.Description,
            SalaryPerMonth = c.SalaryPerMonth,
            VaccantionDays = c.VaccantionDays,
            CompanyId = c.CompanyId,
            Schedule = c.Schedule.WorkingSchedule,
            JobCategory = c.JobCategory.Name,
        }).ToList();
        [HttpGet]
        public async Task<IActionResult> CompanyJobListings()
        {
            IEnumerable<JobListing> jobListings = await companyService.GetAllByJobListingsAsync(GetUserId());
            IEnumerable<JobListingOutputViewModel> viewModel = ToViewModelJobListings(jobListings);
            return View(viewModel);
        }
    }
}
