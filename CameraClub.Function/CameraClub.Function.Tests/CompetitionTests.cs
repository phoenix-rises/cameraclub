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

        [TestInitialize]
        public void Initialize()
        {
            competitionInfo = new CompetitionInfo(this.dbContext.Object, new SaveEntity(), new Translator(), blobServiceClient.Object);

            testCategories = new List<Category>
            {
                new Category { Id = 1, IsDigital = true, Name = "Nature" },
                new Category { Id = 2, IsDigital = true, Name = "People" },
                new Category { Id = 3, IsDigital = true, Name = "Landscape" },
                new Category { Id = 4, IsDigital = false, Name = "Small Print" },
                new Category { Id = 5, IsDigital = false, Name = "Large Print" }
            };

            this.dbContext.Setup(d => d.Categories).ReturnsDbSet(testCategories);
        }

        [TestMethod]
        public void CanGetCompetitions()
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
    }
}