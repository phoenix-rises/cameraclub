using Azure.Storage.Blobs;

using CameraClub.Function.Entities;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

[assembly: FunctionsStartup(typeof(CameraClub.Function.Startup))]
namespace CameraClub.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var blobConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            builder.Services.AddDbContext<CompetitionContext>(optionBuilder => optionBuilder.UseSqlServer("name=SQLConnectionString"));
            builder.Services.AddScoped<SaveEntity>();
            builder.Services.AddScoped<Translator>();
            builder.Services.AddScoped(c => new BlobServiceClient(blobConnectionString));
        }
    }
}