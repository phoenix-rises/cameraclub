using Azure.Storage.Blobs;

using CameraClub.Function.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Moq;
using Moq.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace CameraClub.Function.Tests
{
    public class TestData
    {
        public readonly Mock<CompetitionContext> dbContext = new Mock<CompetitionContext>();
        public readonly Mock<Lazy<BlobServiceClient>> blobServiceClient = new Mock<Lazy<BlobServiceClient>>();
        public readonly Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
        public readonly Mock<ILogger> logger = new Mock<ILogger>();

        public CompetitionInfo competitionInfo { get; private set; }
        public List<Category> testCategories { get; private set; }
        public List<Competition> testCompetitions { get; private set; }
        public List<Judge> testJudges { get; private set; }
        public List<Photographer> testPhotographers { get; private set; }
        public List<Photo> testPhotos { get; private set; }
        public List<Club> testClubs { get; private set; }

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
                new Competition
                {
                    Id = 1,
                    Name = "Summer Competition",
                    HasDigital = true,
                    HasPrint = false,
                    Date = new DateTime(2021, 7, 1),
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 1 },
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 2 },
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 3 }
                    }
                },
                new Competition
                {
                    Id = 2,
                    Name = "Fall Competition",
                    HasDigital = false,
                    HasPrint = true,
                    Date = new DateTime(2021, 10, 1),
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 2 },
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 3 },
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 4 }
                    }
                },
                new Competition
                {
                    Id = 3,
                    Name = "Spring Competition",
                    HasPrint = true,
                    HasDigital = true,
                    Date = new DateTime(2021, 4, 1),
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 3, PhotographerId = 1 },
                        new CompetitionPhotographer { CompetitionId = 3, PhotographerId = 2 }
                    }
                }
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
                new Photographer
                {
                    Id = 1,
                    FirstName = "Samantha",
                    LastName = "Erendale",
                    Email = "sam@gmail.com",
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 1 },
                        new CompetitionPhotographer { CompetitionId = 3, PhotographerId = 1 }
                    }
                },
                new Photographer
                {
                    Id = 2,
                    FirstName = "Samantha",
                    LastName = "Johnson",
                    Email = "sam92@gmail.com",
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 2 },
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 2 },
                        new CompetitionPhotographer { CompetitionId = 3, PhotographerId = 2 }
                    }
                },
                new Photographer
                {
                    Id = 3,
                    FirstName = "Leif",
                    LastName = "Johnson",
                    Email = "leif@gmail.com",
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 1, PhotographerId = 3 },
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 3 }
                    }
                },
                new Photographer
                {
                    Id = 4,
                    FirstName = "Anne",
                    LastName = "McGovern",
                    Email = "anne@gmail.com",
                    CompetitionPhotographer = new List<CompetitionPhotographer>
                    {
                        new CompetitionPhotographer { CompetitionId = 2, PhotographerId = 4 }
                    }
                }
            };

            this.dbContext.Setup(p => p.Photographers).ReturnsDbSet(this.testPhotographers);

            this.testPhotos = new List<Photo>
            {
                new Photo { Id = 1, PhotographerId = 1, CompetitionId = 1, CategoryId = 3, FileName = "test1.jpg", Title = "Test 1", StorageId = new Guid() },
                new Photo { Id = 2, PhotographerId = 2, CompetitionId = 1, CategoryId = 3, FileName = "test2.jpg", Title = "Test 2", StorageId = new Guid() },
                new Photo { Id = 3, PhotographerId = 3, CompetitionId = 1, CategoryId = 1, FileName = "test3.jpg", Title = "Test 3", StorageId = new Guid() },
                new Photo { Id = 4, PhotographerId = 3, CompetitionId = 1, CategoryId = 2, FileName = "test4.jpg", Title = "Test 4", StorageId = new Guid() },
                new Photo { Id = 5, PhotographerId = 2, CompetitionId = 2, CategoryId = 1, FileName = "test5.jpg", Title = "Test 5", StorageId = new Guid() },
                new Photo { Id = 6, PhotographerId = 3, CompetitionId = 2, CategoryId = 2, FileName = "test6.jpg", Title = "Test 6", StorageId = new Guid() },
                new Photo { Id = 7, PhotographerId = 3, CompetitionId = 2, CategoryId = 2, FileName = "test7.jpg", Title = "Test 7", StorageId = new Guid() },
                new Photo { Id = 8, PhotographerId = 4, CompetitionId = 2, CategoryId = 3, FileName = "test8.jpg", Title = "Test 8", StorageId = new Guid() },
                new Photo { Id = 9, PhotographerId = 1, CompetitionId = 3, CategoryId = 1, FileName = "test9.jpg", Title = "Test 9", StorageId = new Guid() },
                new Photo { Id = 10, PhotographerId = 1, CompetitionId = 3, CategoryId = 2, FileName = "test10.jpg", Title = "Test 10", StorageId = new Guid() },
                new Photo { Id = 11, PhotographerId = 2, CompetitionId = 3, CategoryId = 2, FileName = "test11.jpg", Title = "Test 11", StorageId = new Guid() },
                new Photo { Id = 12, PhotographerId = 2, CompetitionId = 3, CategoryId = 3, FileName = "test12.jpg", Title = "Test 12", StorageId = new Guid() }
            };

            this.dbContext.Setup(p => p.Photos).ReturnsDbSet(this.testPhotos);

            this.testClubs = new List<Club> {
                new Club { Id = 1, ClubAssociationNumber = "123", Name = "Champaign Camera Club" }
            };

            this.dbContext.Setup(c => c.Clubs).ReturnsDbSet(this.testClubs);
        }
    }
}