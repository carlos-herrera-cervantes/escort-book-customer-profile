using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka.DependencyInjection;
using Confluent.Kafka;
using EscortBookCustomerProfile.Web.Contexts;
using EscortBookCustomerProfile.Web.Repositories;
using EscortBookCustomerProfile.Web.Services;
using EscortBookCustomerProfile.Web.Handlers;
using EscortBookCustomerProfile.Web.Backgrounds;
using EscortBookCustomerProfile.Web.Extensions;
using EscortBookCustomerProfile.Web.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<EscortBookCustomerProfileContext>(options
    => options.UseNpgsql(PostgresDatabase.CustomerProfile));
builder.Services.AddS3Client();
builder.Services.AddKafkaClient<IKafkaService>(new ProducerConfig
{
    BootstrapServers = Kafka.Servers,
    ClientId = Kafka.ClientId
});
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddTransient<IAvatarRepository, AvatarRepository>();
builder.Services.AddTransient<IIdentificationRepository, IdentificationRepository>();
builder.Services.AddTransient<IPhotoRepository, PhotoRepository>();
builder.Services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
builder.Services.AddTransient<IAWSS3Service, AWSS3Service>();
builder.Services.AddTransient<IKafkaService, KafkaService>();
builder.Services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
builder.Services.AddTransient<IIdentificationPartRepository, IdentificationPartRepository>();
builder.Services.AddSingleton(typeof(IOperationHandler<>), typeof(OperationHandler<>));
builder.Services.AddHostedService<ProfileStatusConsumer>();
builder.Services.AddHostedService<S3Consumer>();
builder.Services.AddHostedService<BlockUserConsumer>();
builder.Services.AddHostedService<DeleteUserConsumer>();

var app = builder.Build();

app.UseHttpLogging();
app.UseRouting();
app.MapControllers();
app.Run();
