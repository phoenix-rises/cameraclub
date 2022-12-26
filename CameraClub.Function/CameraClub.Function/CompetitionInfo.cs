using Azure.Storage.Blobs;

using CameraClub.Function.Contracts;
using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CompetitionEntity = CameraClub.Function.Entities.Competition;
using PhotoEntity = CameraClub.Function.Entities.Photo;
using PhotographerEntity = CameraClub.Function.Entities.Photographer;

namespace CameraClub.Function
{
    public class CompetitionInfo
    {
        private readonly CompetitionContext competitionContext;
        private readonly SaveEntity SaveEntity;
        private readonly Translator translator;
        private readonly Lazy<BlobServiceClient> blobServiceClient;

        public CompetitionInfo(CompetitionContext competitionContext, SaveEntity SaveEntity, Translator translator, Lazy<BlobServiceClient> blobServiceClient)
        {
            this.competitionContext = competitionContext;
            this.SaveEntity = SaveEntity;
            this.translator = translator;
            this.blobServiceClient = blobServiceClient;
        }

        [FunctionName(nameof(GetCompetitions))]
        public IActionResult GetCompetitions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var responseMessage = this.competitionContext.Competitions
                                    .OrderByDescending(c => c.Date)
                                    .ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(SaveCompetition))]
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

        [FunctionName(nameof(GetCategories))]
        public IActionResult GetCategories(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Categories
                                        .OrderBy(c => c.Name)
                                        .ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(SaveCategory))]
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

        [FunctionName(nameof(GetJudges))]
        public IActionResult GetJudges(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Judges
                                        .OrderBy(j => j.FirstName)
                                        .ThenBy(j => j.LastName)
                                        .ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(SaveJudges))]
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

        [FunctionName(nameof(GetPhotographers))]
        public IActionResult GetPhotographers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Photographers
                                        .OrderBy(p => p.FirstName)
                                        .ThenBy(p => p.LastName)
                                        .ToList();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(SavePhotographer))]
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

        [FunctionName(nameof(GetClub))]
        public IActionResult GetClub(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            var responseMessage = this.competitionContext.Clubs.FirstOrDefault();

            return new OkObjectResult(responseMessage);
        }

        [FunctionName(nameof(SaveClub))]
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

        [FunctionName(nameof(GetCompetitionEntries))]
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
                                    .OrderBy(p => p.FirstName)
                                    .ThenBy(p => p.LastName)
                                    .ToList();

            var photos = this.competitionContext.Photos
                            .Where(h => h.CompetitionId == request.CompetitionId)
                            .Select(p => new { p.Id, p.Title, p.PhotographerId, p.CompetitionId, p.CategoryId, p.FileName, p.StorageId })
                            .OrderBy(p => p.Title)
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

        [FunctionName(nameof(SaveCompetitionEntries))]
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
                var entityPhotos = this.competitionContext.Photos.Where(p => p.CompetitionId == request.CompetitionId).ToList();

                this.UpdatePhotographers(request, competitionPhotographers, entityPhotos);

                this.UpdatePhotos(request, entityPhotos);

                this.competitionContext.SaveChanges();

                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error thrown when attempting to SaveCompetitionEntries");
                throw;
            }
        }

        [FunctionName(nameof(UploadPhotoFile))]
        [RequestFormLimits(MultipartBodyLengthLimit = 1000000)]
        public async Task<IActionResult> UploadPhotoFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest request, ILogger log)
        {
            var storageId = Guid.NewGuid().ToString();

            using (var stream = new MemoryStream())
            {
                var file = request.Form.Files.FirstOrDefault();

                if (file == null)
                {
                    log.LogInformation("Request for file upload without any file.");
                    return new BadRequestResult();
                }

                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var blobContainerClient = this.blobServiceClient.Value.GetBlobContainerClient("photos");
                var blobClient = blobContainerClient.GetBlobClient(storageId);

                await blobClient.UploadAsync(stream);
            }

            return new OkObjectResult(
                new
                {
                    storageId
                });
        }

        [FunctionName(nameof(DownloadPhotoFile))]
        public async Task<Stream> DownloadPhotoFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] DownloadPhotoFileRequest request, ILogger log)
        {
            var blobContainerClient = this.blobServiceClient.Value.GetBlobContainerClient("photos");
            var blobClient = blobContainerClient.GetBlobClient(request.StorageId);

            if (!blobClient.Exists())
            {
                log.LogInformation($"Request made to find for image was not found. Request's storage id was {request.StorageId}");
                return null;
            }

            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        private void UpdatePhotographers(SaveCompetitionEntriesRequest request, List<CompetitionPhotographer> competitionPhotographers, List<PhotoEntity> entityPhotos)
        {
            foreach (var photographer in competitionPhotographers.Where(cp => request.Photographers.Any(p => p.Id == cp.PhotographerId && p.IsDeleted)))
            {
                this.competitionContext.Remove(photographer);

                entityPhotos.Where(e => e.PhotographerId == photographer.PhotographerId && e.CompetitionId == request.CompetitionId).ToList()
                    .ForEach(p =>
                        this.competitionContext.Remove(p)
                    );

                request.Photos.Where(w => w.PhotographerId == photographer.PhotographerId).ToList()
                                .ForEach(h =>
                                    request.Photos.Remove(h)
                                );
            }

            foreach (var photographer in request.Photographers.Where(p => !p.IsDeleted && !competitionPhotographers.Any(cp => cp.PhotographerId == p.Id)))
            {
                this.competitionContext.CompetitionPhotographer.Add(new CompetitionPhotographer { CompetitionId = request.CompetitionId, PhotographerId = photographer.Id });
            }
        }

        private void UpdatePhotos(SaveCompetitionEntriesRequest request, List<PhotoEntity> entityPhotos)
        {
            foreach (var requestPhoto in request.Photos)
            {
                var entityPhoto = entityPhotos.FirstOrDefault(p => p.Id == requestPhoto.Id);

                if (entityPhoto != null)
                {
                    if (requestPhoto.IsDeleted)
                    {
                        this.competitionContext.Remove(entityPhoto);
                        continue;
                    }

                    entityPhoto.CategoryId = requestPhoto.CategoryId;
                    entityPhoto.Title = requestPhoto.Title;
                    entityPhoto.FileName = requestPhoto.FileName;
                    entityPhoto.StorageId = requestPhoto.StorageId;
                }
                else if (!requestPhoto.IsDeleted)
                {
                    var newPhoto = new Entities.Photo
                    {
                        CompetitionId = request.CompetitionId,
                        PhotographerId = requestPhoto.PhotographerId,
                        CategoryId = requestPhoto.CategoryId,
                        Title = requestPhoto.Title,
                        FileName = requestPhoto.FileName,
                        StorageId = requestPhoto.StorageId
                    };

                    this.competitionContext.Photos.Add(newPhoto);
                }
            }
        }

        private static IActionResult InvalidRequestResponse<T>(int id, ILogger log)
        {
            log.LogInformation($"Request of type {typeof(T).Name} made with an invalid Id {id}");

            return new BadRequestResult();
        }
    }
}