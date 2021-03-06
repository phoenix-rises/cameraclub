using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraClub.Function.Tests
{
    [TestClass]
    public class GetSaveTests
    {
        private readonly TestData testData = new TestData();

        [TestInitialize]
        public void Initialize()
        {
            this.testData.Initialize();
        }

        [TestMethod]
        public void CanGetCategories()
        {
            var response = this.testData.competitionInfo.GetCategories(this.testData.httpRequest.Object, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

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

            var categoryToUpdate = this.testData.testCategories.First(c => c.Id == request.Id);

            this.testData.dbContext.Setup(d => d.FindAsync<Category>(It.IsAny<int>())).Returns(new ValueTask<Category>(categoryToUpdate));
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCategory(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", categoryToUpdate.Name);
            Assert.IsFalse(categoryToUpdate.IsDigital);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void CanAddCategory()
        {
            var request = new Contracts.SaveCategoryRequest
            {
                IsDigital = true,
                Name = "Added"
            };

            this.testData.dbContext.Setup(d => d.Add(It.IsAny<Category>())).Verifiable();
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCategory(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SaveCategoriesReturnsErrorResponse()
        {
            var request = new Contracts.SaveCategoryRequest
            {
                Id = 99,
                IsDigital = false,
                Name = "Updated"
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Category>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCategory(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }

        [TestMethod]
        public void CanGetCompetitions()
        {
            var response = this.testData.competitionInfo.GetCompetitions(this.testData.httpRequest.Object, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

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

            var competitionToUpdate = this.testData.testCompetitions.First(c => c.Id == 2);

            this.testData.dbContext.Setup(d => d.FindAsync<Competition>(It.IsAny<int>())).Returns(new ValueTask<Competition>(competitionToUpdate));
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCompetition(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", competitionToUpdate.Name);
            Assert.IsFalse(competitionToUpdate.HasDigital);
            Assert.IsFalse(competitionToUpdate.HasPrint);
            Assert.AreEqual(2022, competitionToUpdate.Date.Year);
            Assert.AreEqual(2, competitionToUpdate.Date.Month);
            Assert.AreEqual(10, competitionToUpdate.Date.Day);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SaveCompetitionsReturnsErrorResponse()
        {
            var request = new Contracts.SaveCompetitionRequest
            {
                Id = 99,
                Date = new DateTime(2022, 2, 10),
                Name = "Updated",
                HasDigital = false,
                HasPrint = false
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Competition>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveCompetition(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }

        [TestMethod]
        public void CanGetJudges()
        {
            var response = this.testData.competitionInfo.GetJudges(this.testData.httpRequest.Object, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

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

            var judgeToUpdate = this.testData.testJudges.First(c => c.Id == request.Id);

            this.testData.dbContext.Setup(d => d.FindAsync<Judge>(It.IsAny<int>())).Returns(new ValueTask<Judge>(judgeToUpdate));
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveJudges(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("UpdatedFirst", judgeToUpdate.FirstName);
            Assert.AreEqual("UpdatedLast", judgeToUpdate.LastName);
            Assert.AreEqual("Updated@gmail.com", judgeToUpdate.Email);
            Assert.AreEqual("2227779999", judgeToUpdate.PhoneNumber);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SaveJudgesReturnsErrorResponse()
        {
            var request = new Contracts.SaveJudgeRequest
            {
                Id = 99,
                FirstName = "UpdatedFirst",
                LastName = "UpdatedLast",
                Email = "Updated@gmail.com",
                PhoneNumber = "2227779999"
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Judge>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveJudges(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }

        [TestMethod]
        public void CanGetPhotographers()
        {
            var response = this.testData.competitionInfo.GetPhotographers(this.testData.httpRequest.Object, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

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

            var photographerToUpdate = this.testData.testPhotographers.First(c => c.Id == request.Id);

            this.testData.dbContext.Setup(d => d.FindAsync<Photographer>(It.IsAny<int>())).Returns(new ValueTask<Photographer>(photographerToUpdate));
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SavePhotographer(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("UpdatedFirst", photographerToUpdate.FirstName);
            Assert.AreEqual("UpdatedLast", photographerToUpdate.LastName);
            Assert.AreEqual("Updated@gmail.com", photographerToUpdate.Email);
            Assert.AreEqual("111", photographerToUpdate.CompetitionNumber);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SavePhotographersReturnsErrorResponse()
        {
            var request = new Contracts.SavePhotographerRequest
            {
                Id = 99,
                FirstName = "First",
                LastName = "Last",
                Email = "test@gmail.com",
                CompetitionNumber = "111"
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Photographer>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SavePhotographer(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }

        [TestMethod]
        public void CanGetClub()
        {
            var response = this.testData.competitionInfo.GetClub(this.testData.httpRequest.Object, this.testData.logger.Object);

            var result = response as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

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

            var clubToUpdate = this.testData.testClubs.First(c => c.Id == request.Id);

            this.testData.dbContext.Setup(d => d.FindAsync<Club>(It.IsAny<int>())).Returns(new ValueTask<Club>(clubToUpdate));
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveClub(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as OkResult;

            Assert.IsNotNull(result);

            Assert.AreEqual("Updated", clubToUpdate.Name);
            Assert.AreEqual("contactme@gmail.com", clubToUpdate.ContactEmail);
            Assert.AreEqual("contactme", clubToUpdate.ContactName);
            Assert.AreEqual("222", clubToUpdate.ClubAssociationNumber);

            this.testData.dbContext.Verify();
        }

        [TestMethod]
        public void SaveClubReturnsErrorResponse()
        {
            var request = new Contracts.SaveClubRequest
            {
                Id = 99,
                Name = "update",
                ContactEmail = "test@test.com",
                ContactName = "me",
                ClubAssociationNumber = "999"
            };

            this.testData.dbContext.Setup(d => d.FindAsync<Club>(It.IsAny<int>())).Returns(null);
            this.testData.dbContext.Setup(d => d.SaveChanges()).Verifiable();

            var response = this.testData.competitionInfo.SaveClub(request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            Assert.IsFalse(this.testData.dbContext.Invocations.Any(v => v.Method.Name == "SaveChanges"));
        }
    }
}