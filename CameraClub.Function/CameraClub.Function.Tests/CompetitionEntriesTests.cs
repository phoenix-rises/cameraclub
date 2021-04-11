using CameraClub.Function.Contracts;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraClub.Function.Tests
{
    [TestClass]
    public class CompetitionEntriesTests
    {
        private readonly TestData testData = new TestData();

        [TestInitialize]
        public void Initialize()
        {
            this.testData.Initialize();
        }

        [TestMethod]
        public void CanGetCompetitionEntries()
        {
            var request = new GetCompetitionEntriesRequest { CompetitionId = 1 };

            var response = this.testData.competitionInfo.GetCompetitionEntries(request, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            dynamic entries = result.Value;

            Assert.AreEqual(1, entries.competition.Id);
            Assert.AreEqual("Summer Competition", entries.competition.Name);
            Assert.IsTrue(entries.competition.HasDigital);
            Assert.IsFalse(entries.competition.HasPrint);
            Assert.AreEqual(2021, entries.competition.Date.Year);
            Assert.AreEqual(7, entries.competition.Date.Month);
            Assert.AreEqual(1, entries.competition.Date.Day);


            Assert.AreEqual(3, entries.photographers[0].Id);
            Assert.AreEqual("Leif", entries.photographers[0].FirstName);
            Assert.AreEqual("Johnson", entries.photographers[0].LastName);

            Assert.AreEqual(1, entries.photographers[1].Id);
            Assert.AreEqual("Samantha", entries.photographers[1].FirstName);
            Assert.AreEqual("Erendale", entries.photographers[1].LastName);

            Assert.AreEqual(2, entries.photographers[2].Id);
            Assert.AreEqual("Samantha", entries.photographers[2].FirstName);
            Assert.AreEqual("Johnson", entries.photographers[2].LastName);


            Assert.AreEqual(1, entries.photos[0].Id);
            Assert.AreEqual("Test 1", entries.photos[0].Title);
            Assert.AreEqual(1, entries.photos[0].PhotographerId);
            Assert.AreEqual(3, entries.photos[0].CategoryId);

            Assert.AreEqual(2, entries.photos[1].Id);
            Assert.AreEqual("Test 2", entries.photos[1].Title);
            Assert.AreEqual(2, entries.photos[1].PhotographerId);
            Assert.AreEqual(3, entries.photos[1].CategoryId);

            Assert.AreEqual(3, entries.photos[2].Id);
            Assert.AreEqual("Test 3", entries.photos[2].Title);
            Assert.AreEqual(3, entries.photos[2].PhotographerId);
            Assert.AreEqual(1, entries.photos[2].CategoryId);

            Assert.AreEqual(4, entries.photos[3].Id);
            Assert.AreEqual("Test 4", entries.photos[3].Title);
            Assert.AreEqual(3, entries.photos[3].PhotographerId);
            Assert.AreEqual(2, entries.photos[3].CategoryId);
        }

        [TestMethod]
        public void GetCompetitionEntriesReturnsErrorResponse()
        {
            var request = new GetCompetitionEntriesRequest { CompetitionId = 0 };

            var response = this.testData.competitionInfo.GetCompetitionEntries(request, this.testData.logger.Object);

            var result = response as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void CanSaveCompetitionEntries()
        {
            var request = new SaveCompetitionEntriesRequest
            {
                CompetitionId = 1,
                Photographers = new List<Photographer>
                {
                    new Photographer { Id = 1, IsDeleted = false },
                    new Photographer { Id = 2, IsDeleted = true },
                    new Photographer { Id = 3, IsDeleted = false },
                    new Photographer { Id = 4, IsDeleted = false }
                },
                Photos = new List<Photo>
                {
                    new Photo { Id = 1, PhotographerId = 1, CompetitionId = 1, CategoryId = 3, Title = "Updated", FileName = "changed.jpg", IsDeleted = false },
                    new Photo { Id = 2, PhotographerId = 2, CompetitionId = 1, CategoryId = 3, Title = "Test 2", FileName = "test2.jpg", IsDeleted = false },
                    new Photo { Id = 3, PhotographerId = 3, CompetitionId = 1, CategoryId = 1, Title = "Test 3", FileName = "test3.jpg", IsDeleted = true },
                    new Photo { Id = 4, PhotographerId = 3, CompetitionId = 1, CategoryId = 2, Title = "Test 4", FileName = "test4.jpg", IsDeleted = false },
                    new Photo { Id = -1, PhotographerId = 1, CompetitionId = 1, CategoryId = 2, Title = "Added", FileName = "test-1.jpg", IsDeleted = false }
                }
            };

            var currentCompetition = this.testData.dbContext.Object.Competitions.First(c => c.Id == request.CompetitionId);

            this.testData.dbContext.Setup(d => d.Competitions.FindAsync(It.IsAny<int>())).Returns(new ValueTask<Entities.Competition>(currentCompetition));
            this.testData.dbContext.Setup(d => d.Remove(It.Is<Entities.CompetitionPhotographer>(m => m.PhotographerId == 2))).Verifiable();
            this.testData.dbContext.Setup(d => d.Remove(It.Is<Entities.Photo>(p => p.Id == 2))).Verifiable();
            this.testData.dbContext.Setup(d => d.Remove(It.Is<Entities.Photo>(p => p.Id == 3))).Verifiable();
            this.testData.dbContext.Setup(d => d.Photos.Add(It.Is<Entities.Photo>(p => p.Title == "Added" && p.FileName == "test-1.jpg" && p.PhotographerId == 1 && p.CategoryId == 2))).Verifiable();
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();


            var response = this.testData.competitionInfo.SaveCompetitionEntries(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);


            Assert.IsTrue(currentCompetition.CompetitionPhotographer.Any(p => p.PhotographerId == 1));
            Assert.IsTrue(currentCompetition.CompetitionPhotographer.Any(p => p.PhotographerId == 3));
            
            var photoOne = this.testData.dbContext.Object.Photos.First(p => p.Id == 1);

            Assert.AreEqual(3, photoOne.CategoryId);
            Assert.AreEqual("Updated", photoOne.Title);
            Assert.AreEqual("changed.jpg", photoOne.FileName);

            var photoFour = this.testData.dbContext.Object.Photos.First(p => p.Id == 4);

            Assert.AreEqual(2, photoFour.CategoryId);
            Assert.AreEqual("Test 4", photoFour.Title);
            Assert.AreEqual("test4.jpg", photoFour.FileName);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SaveCompetitionEntriesReturnsErrorResponse()
        {
            var request = new SaveCompetitionEntriesRequest
            {
                CompetitionId = 99,
                Photographers = new List<Photographer> { new Photographer { Id = 1, IsDeleted = false } },
                Photos = new List<Photo> { new Photo { Id = 1, IsDeleted = false } }
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Entities.Competition>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCompetitionEntries(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }
    }
}