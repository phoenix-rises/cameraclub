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
        private readonly UpsertEntity upsertEntity;
        private readonly Translator translator;

        public CompetitionInfo(CompetitionContext competitionContext, UpsertEntity upsertEntity, Translator translator)
        {
            this.competitionContext = competitionContext;
            this.upsertEntity = upsertEntity;
            this.translator = translator;
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
            if (!await this.upsertEntity.Upsert<UpsertCompetitionRequest, Competition>(this.competitionContext, request.Id, request, this.translator.TranslateCompetition))
            {
                return InvalidRequestResponse<UpsertCompetitionRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetCategories")]
        public async Task<IActionResult> GetCategories(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Categories.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpsertCategory")]
        public async Task<IActionResult> UpsertCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] UpsertCategoryRequest request, ILogger log)
        {
            if (!await this.upsertEntity.Upsert<UpsertCategoryRequest, Category>(this.competitionContext, request.Id, request, this.translator.TranslateCategory))
            {
                return InvalidRequestResponse<UpsertCategoryRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetJudges")]
        public async Task<IActionResult> GetJudges(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Judges.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpsertJudges")]
        public async Task<IActionResult> UpsertJudges(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] UpsertJudgeRequest request, ILogger log)
        {
            if (!await this.upsertEntity.Upsert<UpsertJudgeRequest, Judge>(this.competitionContext, request.Id, request, this.translator.TranslateJudge))
            {
                return InvalidRequestResponse<UpsertJudgeRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetPhotographers")]
        public async Task<IActionResult> GetPhotographers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Photographers.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpsertPhotographer")]
        public async Task<IActionResult> UpsertPhotographer(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] UpsertPhotographerRequest request, ILogger log)
        {
            if (!await this.upsertEntity.Upsert<UpsertPhotographerRequest, Photographer>(this.competitionContext, request.Id, request, this.translator.TranslatePhotographer))
            {
                return InvalidRequestResponse<UpsertPhotographerRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetClub")]
        public async Task<IActionResult> GetClub(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Clubs.FirstOrDefault();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("UpsertClub")]
        public async Task<IActionResult> UpsertClub(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] UpsertClubRequest request, ILogger log)
        {
            if (!await this.upsertEntity.Upsert<UpsertClubRequest, Club>(this.competitionContext, request.Id, request, this.translator.TranslateClub))
            {
                return InvalidRequestResponse<UpsertClubRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        private static IActionResult InvalidRequestResponse<T>(int id, ILogger log)
        {
            log.LogInformation($"Request of type {typeof(T).Name} made with an invalid Id {id}");

            return new NotFoundResult();
        }
    }
}