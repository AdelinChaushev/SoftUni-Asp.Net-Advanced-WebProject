namespace JobFinder.Core.Contracs
{
    public interface IPictureServiceInterface
    {
        public Task UploadPictureAsync(byte[] stream, string userId);

        public Task DeletePictureAsync(Guid id,string userId);
    }
}

