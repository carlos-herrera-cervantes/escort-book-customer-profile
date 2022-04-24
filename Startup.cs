using EscortBookCustomerProfile.Backgrounds;
using EscortBookCustomerProfile.Contexts;
using EscortBookCustomerProfile.Handlers;
using EscortBookCustomerProfile.Repositories;
using EscortBookCustomerProfile.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EscortBookCustomerProfile
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<EscortBookCustomerProfileContext>(options
                => options.UseNpgsql(Configuration["ConnectionStrings:Default"]));
            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<IAvatarRepository, AvatarRepository>();
            services.AddTransient<IIdentificationRepository, IdentificationRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
            services.AddTransient<IAWSS3Service, AWSS3Service>();
            services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
            services.AddTransient<IIdentificationPartRepository, IdentificationPartRepository>();
            services.AddSingleton(typeof(IOperationHandler<>), typeof(OperationHandler<>));
            services.AddHostedService<ProfileStatusConsumer>();
            services.AddHostedService<S3Consumer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
