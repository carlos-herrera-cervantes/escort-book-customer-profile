using Microsoft.Extensions.DependencyInjection;
using Amazon.Runtime;
using Amazon.S3;
using EscortBookCustomerProfile.Web.Constants;

namespace EscortBookCustomerProfile.Web.Extensions;

public static class S3Extenions
{
    public static IServiceCollection AddS3Client(this IServiceCollection services)
    {
        var credentials = new BasicAWSCredentials(S3.AccessKey, S3.SecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = S3.Endpoint,
            UseHttp = true,
            ForcePathStyle = true,
            AuthenticationRegion = S3.Region
        };
        var s3Client = new AmazonS3Client(credentials, config);

        services.AddSingleton<IAmazonS3>(_ => s3Client);

        return services;
    }
}
