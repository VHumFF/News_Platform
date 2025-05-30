﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using News_Platform.Services.Interfaces;
using News_Platform.Utilities;

namespace News_Platform.Services.Implementations
{
    public class S3Service : IS3Service
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var awsOptions = new AwsOptions
            {
                AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
                SessionToken = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN"),
                Region = configuration["AWS:Region"],
                BucketName = configuration["AWS:BucketName"]
            };

            AWSCredentials credentials;

            if (!string.IsNullOrEmpty(awsOptions.SessionToken))
            {
                credentials = new SessionAWSCredentials(
                    awsOptions.AccessKey,
                    awsOptions.SecretKey,
                    awsOptions.SessionToken
                );
            }
            else
            {
                credentials = new BasicAWSCredentials(
                    awsOptions.AccessKey,
                    awsOptions.SecretKey
                );
            }

            var region = RegionEndpoint.GetBySystemName(awsOptions.Region);

            var config = new AmazonS3Config
            {
                RegionEndpoint = region,
                SignatureVersion = "4",
                ForcePathStyle = true
            };

            _bucketName = awsOptions.BucketName;

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
                Expires = DateTime.UtcNow.AddHours(8).Add(expirationDuration),
                ContentType = contentType
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}
