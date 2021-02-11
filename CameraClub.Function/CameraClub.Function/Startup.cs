using CameraClub.Function.Entities;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CameraClub.Function.Startup))]
namespace CameraClub.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<CompetitionContext>(optionBuilder => optionBuilder.UseSqlServer("name=SQLConnectionString"));
            builder.Services.AddScoped<UpsertEntity>();
            builder.Services.AddScoped<Translator>();
        }
    }
}