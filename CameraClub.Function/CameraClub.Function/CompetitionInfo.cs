using CameraClub.Function.Contracts;
using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

using CompetitionEntity = CameraClub.Function.Entities.Competition;
using PhotographerEntity = CameraClub.Function.Entities.Photographer;

namespace CameraClub.Function
{
    public class CompetitionInfo
    {
        private readonly CompetitionContext competitionContext;
        private readonly SaveEntity SaveEntity;
        private readonly Translator translator;

        public CompetitionInfo(CompetitionContext competitionContext, SaveEntity SaveEntity, Translator translator)
        {
            this.competitionContext = competitionContext;
            this.SaveEntity = SaveEntity;
            this.translator = translator;
        }

        [FunctionName("GetCompetitions")]
        public IActionResult GetCompetitions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var responseMessage = this.competitionContext.Competitions.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SaveCompetition")]
        public async Task<IActionResult> SaveCompetition(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] SaveCompetitionRequest request, ILogger log)
        {
            if (!await this.SaveEntity.Save<SaveCompetitionRequest, CompetitionEntity>(this.competitionContext, request.Id, request, this.translator.TranslateCompetition))
            {
                return InvalidRequestResponse<SaveCompetitionRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetCategories")]
        public IActionResult GetCategories(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Categories.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SaveCategory")]
        public async Task<IActionResult> SaveCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] SaveCategoryRequest request, ILogger log)
        {
            if (!await this.SaveEntity.Save<SaveCategoryRequest, Category>(this.competitionContext, request.Id, request, this.translator.TranslateCategory))
            {
                return InvalidRequestResponse<SaveCategoryRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetJudges")]
        public IActionResult GetJudges(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Judges.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SaveJudges")]
        public async Task<IActionResult> SaveJudges(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] SaveJudgeRequest request, ILogger log)
        {
            if (!await this.SaveEntity.Save<SaveJudgeRequest, Judge>(this.competitionContext, request.Id, request, this.translator.TranslateJudge))
            {
                return InvalidRequestResponse<SaveJudgeRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetPhotographers")]
        public IActionResult GetPhotographers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Photographers.ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SavePhotographer")]
        public async Task<IActionResult> SavePhotographer(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] SavePhotographerRequest request, ILogger log)
        {
            if (!await this.SaveEntity.Save<SavePhotographerRequest, PhotographerEntity>(this.competitionContext, request.Id, request, this.translator.TranslatePhotographer))
            {
                return InvalidRequestResponse<SavePhotographerRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetClub")]
        public IActionResult GetClub(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Clubs.FirstOrDefault();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("SaveClub")]
        public async Task<IActionResult> SaveClub(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", "post", Route = null)] SaveClubRequest request, ILogger log)
        {
            if (!await this.SaveEntity.Save<SaveClubRequest, Club>(this.competitionContext, request.Id, request, this.translator.TranslateClub))
            {
                return InvalidRequestResponse<SaveClubRequest>(request.Id.Value, log);
            }

            this.competitionContext.SaveChanges();

            return new OkResult();
        }

        [FunctionName("GetCompetitionEntries")]
        public IActionResult GetCompetitionEntries(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] GetCompetitionEntriesRequest request, ILogger log)
        {
            var competition = this.competitionContext.Competitions
                                .Where(c => c.Id == request.CompetitionId)
                                .Select(s => new { s.Id, s.Name, s.HasDigital, s.HasPrint, s.Date })
                                .FirstOrDefault();

            if (competition == null)
            {
                return InvalidRequestResponse<GetCompetitionEntriesRequest>(request.CompetitionId, log);
            }

            var photographers = this.competitionContext.Photographers
                                    .Where(p => p.CompetitionPhotographer.Any(c => c.CompetitionId == request.CompetitionId))
                                    .Select(n => new { n.Id, n.FirstName, n.LastName, n.Email, n.ClubNumber, n.CompetitionNumber })
                                    .ToList();

            var photos = this.competitionContext.Photos
                            .Where(h => h.CompetitionId == request.CompetitionId)
                            .Select(p => new { p.Id, p.Title, p.PhotographerId, p.CompetitionId, p.CategoryId, p.StorageId })
                            .ToList();

            return new OkObjectResult(
                    new
                    {
                        competition,
                        photographers,
                        photos
                    }
                );
        }

        [FunctionName("SaveCompetitionEntries")]
        public async Task<IActionResult> SaveCompetitionEntries(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] SaveCompetitionEntriesRequest request, ILogger log)
        {
            try
            {
                var competition = await this.competitionContext.Competitions.FindAsync(request.CompetitionId);

                if (competition == null)
                {
                    return InvalidRequestResponse<SaveCompetitionEntriesRequest>(request.CompetitionId, log);
                }

                var competitionPhotographers = this.competitionContext.CompetitionPhotographer.Where(cp => cp.CompetitionId == request.CompetitionId).ToList();

                foreach (var photographer in competitionPhotographers.Where(cp => request.Photographers.Any(p => p.Id == cp.PhotographerId && p.IsDeleted)))
                {
                    this.competitionContext.Remove(photographer);
                }

                foreach (var photographer in request.Photographers.Where(p => !p.IsDeleted && !competitionPhotographers.Any(cp => cp.PhotographerId == p.Id)))
                {
                    this.competitionContext.CompetitionPhotographer.Add(new CompetitionPhotographer { CompetitionId = request.CompetitionId, PhotographerId = photographer.Id });
                }

                var photos = this.competitionContext.Photos.Where(p => p.CompetitionId == request.CompetitionId).ToList();

                foreach (var photo in photos.Where(p => request.Photos.Any(ph => ph.Id == p.Id && ph.IsDeleted)))
                {
                    this.competitionContext.Remove(photo);
                }

                foreach (var photo in request.Photos.Where(ph => !ph.IsDeleted && !photos.Any(p => p.Id == ph.Id)))
                {
                    var newPhoto = new Entities.Photo
                    {
                        CompetitionId = request.CompetitionId,
                        PhotographerId = photo.PhotographerId,
                        CategoryId = photo.CategoryId,
                        Title = photo.Title
                    };
                    this.competitionContext.Photos.Add(newPhoto);
                }

                await this.competitionContext.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error thrown when attempting to SaveCompetitionEntries");
                throw;
            }
        }

        private static IActionResult InvalidRequestResponse<T>(int id, ILogger log)
        {
            log.LogInformation($"Request of type {typeof(T).Name} made with an invalid Id {id}");

            return new NotFoundResult();
        }
    }
}