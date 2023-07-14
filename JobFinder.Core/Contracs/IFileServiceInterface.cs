namespace JobFinder.Core.Contracs
{
    public interface IFileServiceInterface
    {
        public Task UploadPictureAsync(MemoryStream stream, Guid companyId);

        public Task DeletePictureAsync(Guid id,string userId);

        public Task UploadResumeAsync(MemoryStream stream, string userId);

        public Task DeleteResumeAsync( string userId);

      
    }
}

