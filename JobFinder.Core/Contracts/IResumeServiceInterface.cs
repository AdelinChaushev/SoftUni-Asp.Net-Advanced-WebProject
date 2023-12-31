﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Contracts
{
    public interface IResumeServiceInterface
    {
        public Task UploadResumeAsync(byte[] bytes, string userId);

        public Task DeleteResumeAsync(string userId);

        public Task<string> GetResumePathByIdAsync(Guid? id);

        public  Task<string> GetResumePathByUserIdAsync(string id);
    }
}
