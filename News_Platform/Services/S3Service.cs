using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;

namespace News_Platform.Services
{
    public class S3Service : IS3Service
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var profileName = "default";
            var credentials = new StoredProfileAWSCredentials(profileName);
            var region = RegionEndpoint.USEast1;

            _bucketName = configuration["AWS:BucketName"];

            var config = new AmazonS3Config
            {
                RegionEndpoint = region,
                SignatureVersion = "4",
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(credentials, config);
        }

        public string GeneratePresignedUrl(string objectKey, TimeSpan expirationDuration, string contentType)
        {
            if (contentType == "image/jpg")
            {
                contentType = "image/jpeg";
            }

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.Add(expirationDuration),
                ContentType = contentType
            };

            return _s3Client.GetPreSignedURL(request);
        }




    }
}
