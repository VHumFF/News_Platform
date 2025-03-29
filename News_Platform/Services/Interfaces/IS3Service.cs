namespace News_Platform.Services.Interfaces
{
    public interface IS3Service
    {
        string GeneratePresignedUrl(string objectKey, TimeSpan expirationDuration, string contentType);
    }
}
