using CameraClub.Function.Contracts;
using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace CameraClub.Function
{
    public class CompetitionInfo
    {
        private readonly CompetitionContext competitionContext;

        public CompetitionInfo(CompetitionContext competitionContext)
        {
            this.competitionContext = competitionContext;
        }

        [FunctionName("GetCompetitions")]
        public async Task<IActionResult> GetCompetitions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var responseMessage = this.competitionContext.Competitions.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpsertCompetition")]
        public async Task<IActionResult> UpsertCompetition(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] UpsertCompetitionRequest request, ILogger log)
        {
            Competition entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Competition>(request.Id.Value);

                if (entity == null)
                {
                    log.LogInformation($"Request made with invalid Id {request.Id}");

                    return new NotFoundResult();
                }

                this.TranslateCompetition(request, entity);
            }
            else
            {
                entity = new Competition();
                this.TranslateCompetition(request, entity);

                this.competitionContext.Add(entity);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        private void TranslateCompetition(UpsertCompetitionRequest request, Competition entity)
        {
            entity.Name = request.Name;
            entity.Date = request.Date;
            entity.HasDigital = request.HasDigital;
            entity.HasPrint = request.HasPrint;
        }
    }
}
