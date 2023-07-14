﻿using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class CompanyController : BaseController
    { 
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }

       

        //private Task<CompanyOutputViewModel> ToViewModel(Company company)
        //{
        //    CompanyInputViewModel companyViewModel = new CompanyInputViewModel();
        //    companyViewModel.OwnerId = company.OwnerId;
        //    companyViewModel.CompanyDescription = company.CompanyDescription;
        //    companyViewModel.CompanyName = company.CompanyName;
        //    companyViewModel.Id = company.Id;
        //}
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
         => new()
            {
                CompanyDescription = compnayViewModel.CompanyDescription,
                CompanyName = compnayViewModel.CompanyName,
            };
        
    }
}
