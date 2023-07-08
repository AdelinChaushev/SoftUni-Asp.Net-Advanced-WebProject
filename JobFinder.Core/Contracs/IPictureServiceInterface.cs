namespace JobFinder.Core.Contracs
{
    public interface IPictureServiceInterface
    {
        public Task UploadPictureAsync(MemoryStream stream, Guid companyId);

        public Task DeletePictureAsync(Guid id,string userId);
    }
}

