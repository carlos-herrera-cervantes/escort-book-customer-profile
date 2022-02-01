using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Services
{
    public class AWSS3Service : IAWSS3Service
    {
        #region snippet_Properties

        private readonly IAmazonS3 _s3Client;

        private readonly string _bucketName;

        private readonly string _endpoint;

        #endregion

        #region snippet_Constructors

        public AWSS3Service(IConfiguration configuration)
        {
            var s3Section = configuration.GetSection("AWS").GetSection("S3");
            var accessKey = s3Section.GetSection("AccessKey").Value;
            var secretKey = s3Section.GetSection("SecretKey").Value;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = s3Section.GetSection("Endpoint").Value,
                UseHttp = true,
                ForcePathStyle = true,
                AuthenticationRegion = s3Section.GetSection("Region").Value
            });
            _bucketName = s3Section.GetSection("Name").Value;
            _endpoint = s3Section.GetSection("Endpoint").Value;
        }

        #endregion

        #region snippet_ActionMethods

        public async Task<string> PutObjectAsync(string key, string profileId, Stream imageStream)
        {
            var request = new PutObjectRequest
            {
                InputStream = imageStream,
                BucketName = _bucketName,
                Key = $"{profileId}/{key}"
            };

            await _s3Client.PutObjectAsync(request);
            return $"{_endpoint}/{_bucketName}/{profileId}/{key}";
        }

        #endregion
    }
}
