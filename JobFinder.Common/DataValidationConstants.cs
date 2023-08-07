using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Common
{
    public class DataValidationConstants
    {
        public static class Company
        {
            public const int NameMinLenght = 3;
            public const int NameMaxLenght = 35;

            public const int DescriptionMinLenght = 25;
            public const int DescriptionMaxLenght = 800;
        }
        public static class JobCategory
        {
            public const int NameMinLenght = 3;
            public const int NameMaxLenght = 35;
        }
        public static class Schedule
        {
            public const int NameMinLenght = 2;
            public const int NameMaxLenght = 35;
        }
        public static class JobListing
        {
            public const int NameMinLenght = 3;
            public const int NameMaxLenght = 35;

            public const int DescriptionMinLenght = 30;
            public const int DescriptionMaxLenght = 500;

            public const int MinSalaryPerMonth = 0;
            public const int MaxSalaryPerMonth = 1_000_000_000;

            public const int MinVaccaintionDays = 1;
            public const int MaxVaccaintionDays = 100;

        }


    }
}
