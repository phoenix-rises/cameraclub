using Azure;
using Azure.Storage.Blobs;

using CameraClub.Function.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraClub.Function.Tests
{
    [TestClass]
    public class PhotoFileTests
    {
        private readonly TestData testData = new TestData();
        private readonly string storageId = new Guid().ToString();

        private readonly Mock<BlobContainerClient> blobContainerClient = new Mock<BlobContainerClient>();
        private readonly Mock<BlobClient> blobClient = new Mock<BlobClient>();
        private readonly Mock<BlobServiceClient> serviceClient = new Mock<BlobServiceClient>();

        [TestInitialize]
        public void Initialize()
        {
            this.blobContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(blobClient.Object).Verifiable();
            this.serviceClient.Setup(b => b.GetBlobContainerClient("photos")).Returns(blobContainerClient.Object).Verifiable();

            this.testData.SetBlobServiceClient(this.serviceClient);

            this.testData.Initialize();
        }

        [TestMethod]
        public void CanDownloadFile()
        {
            var request = new DownloadPhotoFileRequest
            {
                StorageId = this.storageId
            };

            var originalContent = "This is a test";
            var buffer = Encoding.Default.GetBytes(originalContent);

            var mockResponse = new Mock<Response>();
            this.blobClient.Setup(b => b.Exists(It.IsAny<System.Threading.CancellationToken>())).Returns(Response.FromValue(true, mockResponse.Object));
            this.blobClient.Setup(b => b.DownloadToAsync(It.IsAny<MemoryStream>()))
                .Returns<Stream>((s) =>
                {
                    s.Write(buffer, 0, buffer.Length);

                    return Task.FromResult(mockResponse.Object);
                })
                .Verifiable();

            var fileData = this.testData.competitionInfo.DownloadPhotoFile(request, this.testData.logger.Object).GetAwaiter();

            var memoryStream = fileData.GetResult() as MemoryStream;

            Assert.AreEqual(0, memoryStream.Position);

            using (var streamReader = new StreamReader(memoryStream))
            {
                var contentReturned = streamReader.ReadToEnd();

                Assert.AreEqual(contentReturned, originalContent);
            }

            this.serviceClient.Verify();
            this.blobClient.Verify();
            this.blobContainerClient.Verify();
        }

        [TestMethod]
        public void ErrorWhenFileDoesNotExist()
        {
            var request = new DownloadPhotoFileRequest
            {
                StorageId = this.storageId
            };

            var mockResponse = new Mock<Response>();
            this.blobClient.Setup(b => b.Exists(It.IsAny<System.Threading.CancellationToken>())).Returns(Response.FromValue(false, mockResponse.Object));

            var fileData = this.testData.competitionInfo.DownloadPhotoFile(request, this.testData.logger.Object).GetAwaiter();

            var result = fileData.GetResult();

            Assert.IsNull(result);

            this.serviceClient.Verify();
            this.blobClient.Verify();
            this.blobContainerClient.Verify();
        }

        [TestMethod]
        public void CanUploadFile()
        {
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((s, ct) =>
                {
                    var buffer = Encoding.Default.GetBytes("This is a test");
                    s.Write(buffer, 0, buffer.Length);

                    return Task.CompletedTask;
                })
                .Verifiable();

            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(c => c.Files.GetEnumerator()).Returns(new List<IFormFile> { formFile.Object }.GetEnumerator());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Form = formCollection.Object;

            this.blobClient.Setup(b => b.UploadAsync(It.IsAny<MemoryStream>())).Verifiable();


            var response = this.testData.competitionInfo.UploadPhotoFile(httpContext.Request, this.testData.logger.Object).GetAwaiter();


            var result = response.GetResult() as OkObjectResult;

            Assert.IsNotNull(result);

            dynamic anonReturnValue = result.Value;

            Assert.IsTrue(Guid.TryParse(anonReturnValue.storageId, out Guid newGuid));

            formFile.Verify();
            this.serviceClient.Verify();
            this.blobContainerClient.Verify();
            this.blobClient.Verify(v => v.UploadAsync(It.IsAny<Stream>()), Times.Once);
        }

        [TestMethod]
        public void EmptyFileReturnsError()
        {
            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(c => c.Files.GetEnumerator()).Returns(new List<IFormFile>().GetEnumerator());

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Form = formCollection.Object;

            var response = this.testData.competitionInfo.UploadPhotoFile(httpContext.Request, this.testData.logger.Object).GetAwaiter();

            var result = response.GetResult() as BadRequestResult;

            Assert.IsNotNull(result);

            this.serviceClient.Verify(v => v.GetBlobContainerClient("photos"), Times.Never);
            this.blobContainerClient.Verify(v => v.GetBlobClient(this.storageId), Times.Never);
        }
    }
}