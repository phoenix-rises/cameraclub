using CameraClub.Function.Contracts;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}