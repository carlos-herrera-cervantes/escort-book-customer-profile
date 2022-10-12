using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EscortBookCustomerProfile.Services;

public class AWSS3Service : IAWSS3Service
{
    #region snippet_Properties

    private readonly IAmazonS3 _s3Client;

    private readonly string _bucketName;

    private readonly string _endpoint;

    #endregion

    #region snippet_Constructors

    public AWSS3Service()
    {
        var accessKey = Environment.GetEnvironmentVariable("AWS_S3_ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("AWS_S3_SECRET_KEY");
        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT"),
            UseHttp = true,
            ForcePathStyle = true,
            AuthenticationRegion = Environment.GetEnvironmentVariable("AWS_S3_REGION")
        });
        _bucketName = Environment.GetEnvironmentVariable("AWS_S3_NAME");
        _endpoint = Environment.GetEnvironmentVariable("AWS_S3_ENDPOINT");
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

    public async Task DeleteObjectAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    #endregion
}
