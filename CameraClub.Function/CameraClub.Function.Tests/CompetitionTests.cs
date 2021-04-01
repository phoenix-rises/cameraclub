using Azure.Storage.Blobs;

using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraClub.Function.Tests
{
    [TestClass]
    public class CompetitionTests
    {
        private readonly Mock<CompetitionContext> dbContext = new Mock<CompetitionContext>();
        private readonly Mock<Lazy<BlobServiceClient>> blobServiceClient = new Mock<Lazy<BlobServiceClient>>();
        private readonly Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
        private readonly Mock<ILogger> logger = new Mock<ILogger>();

        private CompetitionInfo competitionInfo;
        private List<Category> testCategories;
        private List<Competition> testCompetitions;
        private List<Judge> testJudges;
        private List<Photographer> testPhotographers;
        private List<Club> testClubs;

        [TestInitialize]
        public void Initialize()
        {
            this.competitionInfo = new CompetitionInfo(this.dbContext.Object, new SaveEntity(), new Translator(), this.blobServiceClient.Object);

            this.testCategories = new List<Category>
            {
                new Category { Id = 1, IsDigital = true, Name = "Nature" },
                new Category { Id = 2, IsDigital = true, Name = "People" },
                new Category { Id = 3, IsDigital = true, Name = "Landscape" },
                new Category { Id = 4, IsDigital = false, Name = "Small Print" },
                new Category { Id = 5, IsDigital = false, Name = "Large Print" }
            };

            this.dbContext.Setup(d => d.Categories).ReturnsDbSet(this.testCategories);

            this.testCompetitions = new List<Competition>
            {
                new Competition { Id = 1, Name = "Summer Competition", HasDigital = true, Date = new DateTime(2021, 7, 1) },
                new Competition { Id = 2, Name = "Fall Competition", HasPrint = true, Date = new DateTime(2021, 10, 1) },
                new Competition { Id = 3, Name = "Spring Competition", HasPrint = true, HasDigital = true, Date = new DateTime(2021, 4, 1) }
            };

            this.dbContext.Setup(d => d.Competitions).ReturnsDbSet(this.testCompetitions);

            this.testJudges = new List<Judge>
            {
                new Judge { Id = 1, FirstName = "Bob", LastName = "Smith", Email = "bob@gmail.com" },
                new Judge { Id = 2, FirstName = "Suzy", LastName = "Jones", Email = "suzy@gmail.com" },
                new Judge { Id = 3, FirstName = "John", LastName = "Smith", Email = "john@gmail.com" },
                new Judge { Id = 4, FirstName = "Joe", LastName = "Edwards", Email = "joe@gmail.com" }
            };

            this.dbContext.Setup(j => j.Judges).ReturnsDbSet(this.testJudges);

            this.testPhotographers = new List<Photographer>
            {
                new Photographer { Id = 1, FirstName = "Samantha", LastName = "Erendale", Email = "sam@gmail.com" },
                new Photographer { Id = 2, FirstName = "Samantha", LastName = "Johnson", Email = "sam92@gmail.com" },
                new Photographer { Id = 3, FirstName = "Leif", LastName = "Johnson", Email = "leif@gmail.com" },
                new Photographer { Id = 4, FirstName = "Anne", LastName = "McGovern", Email = "anne@gmail.com" }
            };

            this.dbContext.Setup(p => p.Photographers).ReturnsDbSet(this.testPhotographers);

            this.testClubs = new List<Club> {
                new Club { Id = 1, ClubAssociationNumber = "123", Name = "Champaign Camera Club" }
            };

            this.dbContext.Setup(c => c.Clubs).ReturnsDbSet(this.testClubs);
        }

        [TestMethod]
        public void CanGetCategories()
        {
            var response = this.competitionInfo.GetCategories(this.httpRequest.Object, this.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var categories = result.Value as List<Category>;

            Assert.AreEqual(5, categories.Count);
            Assert.AreEqual(3, categories[0].Id);
            Assert.AreEqual(4, categories[4].Id);
        }

        [TestMethod]
        public void CanSaveCategories()
        {
            var request = new Contracts.SaveCategoryRequest
            {
                Id = 2,
                IsDigital = false,
                Name = "Updated"
            };

            var updatedCategory = this.testCategories.Where(c => c.Id == 2).First();

            this.dbContext.Setup(d => d.FindAsync<Category>(It.IsAny<int>())).Returns(new ValueTask<Category>(updatedCategory));
            this.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.competitionInfo.SaveCategory(request, this.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", updatedCategory.Name);
            Assert.IsFalse(updatedCategory.IsDigital);

            this.dbContext.Verify();
        }

        [TestMethod]
        public void CanGetCompetitions()
        {
            var response = this.competitionInfo.GetCompetitions(this.httpRequest.Object, this.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var competitions = result.Value as List<Competition>;

            Assert.AreEqual(3, competitions.Count);
            Assert.AreEqual(2, competitions[0].Id);
            Assert.AreEqual(1, competitions[1].Id);
            Assert.AreEqual(3, competitions[2].Id);

            Assert.AreEqual(2021, competitions[0].Date.Year);
            Assert.AreEqual(10, competitions[0].Date.Month);
            Assert.AreEqual(1, competitions[0].Date.Day);
        }

        [TestMethod]
        public void CanSaveCompetitions()
        {
            var request = new Contracts.SaveCompetitionRequest
            {
                Id = 3,
                Date = new DateTime(2022, 2, 10),
                Name = "Updated",
                HasDigital = false,
                HasPrint = false
            };

            var updatedCompetition = this.testCompetitions.Where(c => c.Id == 2).First();

            this.dbContext.Setup(d => d.FindAsync<Competition>(It.IsAny<int>())).Returns(new ValueTask<Competition>(updatedCompetition));
            this.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.competitionInfo.SaveCompetition(request, this.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", updatedCompetition.Name);
            Assert.IsFalse(updatedCompetition.HasDigital);
            Assert.IsFalse(updatedCompetition.HasPrint);
            Assert.AreEqual(2022, updatedCompetition.Date.Year);
            Assert.AreEqual(2, updatedCompetition.Date.Month);
            Assert.AreEqual(10, updatedCompetition.Date.Day);

            this.dbContext.Verify();
        }

        [TestMethod]
        public void CanGetJudges()
        {
            var response = this.competitionInfo.GetJudges(this.httpRequest.Object, this.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var judges = result.Value as List<Judge>;

            Assert.AreEqual(4, judges.Count);
            Assert.AreEqual(1, judges[0].Id);
            Assert.AreEqual(4, judges[1].Id);
            Assert.AreEqual(3, judges[2].Id);
            Assert.AreEqual(2, judges[3].Id);
        }

        [TestMethod]
        public void CanSaveJudges()
        {
            var request = new Contracts.SaveJudgeRequest
            {
                Id = 3,
                FirstName = "UpdatedFirst",
                LastName = "UpdatedLast",
                Email = "Updated@gmail.com",
                PhoneNumber = "2227779999"
            };

            var updatedJudge = this.testJudges.Where(c => c.Id == 3).First();

            this.dbContext.Setup(d => d.FindAsync<Judge>(It.IsAny<int>())).Returns(new ValueTask<Judge>(updatedJudge));
            this.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.competitionInfo.SaveJudges(request, this.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("UpdatedFirst", updatedJudge.FirstName);
            Assert.AreEqual("UpdatedLast", updatedJudge.LastName);
            Assert.AreEqual("Updated@gmail.com", updatedJudge.Email);
            Assert.AreEqual("2227779999", updatedJudge.PhoneNumber);

            this.dbContext.Verify();
        }

        [TestMethod]
        public void CanGetPhotographers()
        {
            var response = this.competitionInfo.GetPhotographers(this.httpRequest.Object, this.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var photographers = result.Value as List<Photographer>;

            Assert.AreEqual(4, photographers.Count);
            Assert.AreEqual(4, photographers[0].Id);
            Assert.AreEqual(3, photographers[1].Id);
            Assert.AreEqual(1, photographers[2].Id);
            Assert.AreEqual(2, photographers[3].Id);
        }

        [TestMethod]
        public void CanSavePhotographer()
        {
            var request = new Contracts.SavePhotographerRequest
            {
                Id = 3,
                FirstName = "UpdatedFirst",
                LastName = "UpdatedLast",
                Email = "Updated@gmail.com",
                CompetitionNumber = "111"
            };

            var updatedPhotographer = this.testPhotographers.Where(c => c.Id == 3).First();

            this.dbContext.Setup(d => d.FindAsync<Photographer>(It.IsAny<int>())).Returns(new ValueTask<Photographer>(updatedPhotographer));
            this.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.competitionInfo.SavePhotographer(request, this.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("UpdatedFirst", updatedPhotographer.FirstName);
            Assert.AreEqual("UpdatedLast", updatedPhotographer.LastName);
            Assert.AreEqual("Updated@gmail.com", updatedPhotographer.Email);
            Assert.AreEqual("111", updatedPhotographer.CompetitionNumber);

            this.dbContext.Verify();
        }

        [TestMethod]
        public void CanGetClub()
        {
            var response = this.competitionInfo.GetClub(this.httpRequest.Object, this.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == 200);

            var club = result.Value as Club;

            Assert.AreEqual(1, club.Id);
            Assert.AreEqual("Champaign Camera Club", club.Name);
        }

        [TestMethod]
        public void CanSaveClub()
        {
            var request = new Contracts.SaveClubRequest
            {
                Id = 1,
                Name = "Updated",
                ContactEmail = "contactme@gmail.com",
                ContactName = "contactme",
                ClubAssociationNumber = "222"
            };

            var updatedClub = this.testClubs.Where(c => c.Id == 1).First();

            this.dbContext.Setup(d => d.FindAsync<Club>(It.IsAny<int>())).Returns(new ValueTask<Club>(updatedClub));
            this.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.competitionInfo.SaveClub(request, this.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", updatedClub.Name);
            Assert.AreEqual("contactme@gmail.com", updatedClub.ContactEmail);
            Assert.AreEqual("contactme", updatedClub.ContactName);
            Assert.AreEqual("222", updatedClub.ClubAssociationNumber);

            this.dbContext.Verify();
        }
    }
}