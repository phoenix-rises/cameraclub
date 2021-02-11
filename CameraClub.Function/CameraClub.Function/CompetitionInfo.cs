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
            var translator = new Translator();
            Competition entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Competition>(request.Id.Value);

                if (entity == null)
                {
                    return InvalidRequestResponse(request.Id.Value, log);
                }

                translator.TranslateCompetition(request, entity);
            }
            else
            {
                entity = new Competition();
                translator.TranslateCompetition(request, entity);

                this.competitionContext.Add(entity);
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
            var translator = new Translator();
            Category entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Category>(request.Id.Value);

                if (entity == null)
                {
                    return InvalidRequestResponse(request.Id.Value, log);
                }

                translator.TranslateCategory(request, entity);
            }
            else
            {
                entity = new Category();
                translator.TranslateCategory(request, entity);

                this.competitionContext.Add(entity);
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
            var translator = new Translator();
            Judge entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Judge>(request.Id.Value);

                if (entity == null)
                {
                    return InvalidRequestResponse(request.Id.Value, log);
                }

                translator.TranslateJudge(request, entity);
            }
            else
            {
                entity = new Judge();
                translator.TranslateJudge(request, entity);

                this.competitionContext.Add(entity);
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
            var translator = new Translator();
            Photographer entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Photographer>(request.Id.Value);

                if (entity == null)
                {
                    return InvalidRequestResponse(request.Id.Value, log);
                }

                translator.TranslatePhotographer(request, entity);
            }
            else
            {
                entity = new Photographer();
                translator.TranslatePhotographer(request, entity);

                this.competitionContext.Add(entity);
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
            var translator = new Translator();
            Club entity;

            if (request.Id.HasValue)
            {
                entity = await this.competitionContext.FindAsync<Club>(request.Id.Value);

                if (entity == null)
                {
                    return InvalidRequestResponse(request.Id.Value, log);
                }

                translator.TranslateClub(request, entity);
            }
            else
            {
                entity = new Club();
                translator.TranslateClub(request, entity);

                this.competitionContext.Add(entity);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        private static IActionResult InvalidRequestResponse(int id, ILogger log)
        {
            log.LogInformation($"Request made with an invalid Id {id}");

            return new NotFoundResult();
        }
    }
}