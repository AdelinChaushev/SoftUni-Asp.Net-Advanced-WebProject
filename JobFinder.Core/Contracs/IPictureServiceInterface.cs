namespace JobFinder.Core.Contracs
{
    public interface IPictureServiceInterface
    {
        public Task UploadFiles(MemoryStream stream, Guid companyId);

        public Task<string> DeleteFile(Guid id);
    }
}

