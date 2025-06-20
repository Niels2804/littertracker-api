using Microsoft.AspNetCore.Mvc;
using Moq;
using trashtracker_api.Controllers;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace Trashtracker_api.test
{
    [TestClass]
    public class LitterControllerTests
    {
        private Mock<ILitterRepository> _litterRepository;
        private LitterController _controller;

        [TestInitialize]
        public void Setup()
        {
            _litterRepository = new Mock<ILitterRepository>();
            _controller = new LitterController(_litterRepository.Object);
        }

        [TestMethod]
        public async Task GetByLitterId_LitterExists_ReturnsOkObjectResult()
        {
            // Arrange
            var litterId = Guid.NewGuid().ToString();
            var expectedLitter = new Litter { Id = litterId };
            _litterRepository.Setup(repo => repo.GetByLitterIdAsync(litterId))
                             .ReturnsAsync(expectedLitter);

            // Act
            var result = await _controller.GetByLitterId(litterId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedLitter = okResult.Value as Litter;
            Assert.IsNotNull(returnedLitter);
            Assert.AreEqual(expectedLitter.Id, returnedLitter.Id);
        }

        [TestMethod]
        public async Task GetByLitterId_LitterDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var litterId = Guid.NewGuid().ToString();
            _litterRepository.Setup(repo => repo.GetByLitterIdAsync(litterId))
                             .ReturnsAsync((Litter)null);

            // Act
            var result = await _controller.GetByLitterId(litterId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetAllLitter_LitterExists_ReturnsOkObjectResult()
        {
            // Arrange
            var litters = new List<Litter>
            {
                new Litter { Id = Guid.NewGuid().ToString() },
                new Litter { Id = Guid.NewGuid().ToString() }
            };
            _litterRepository.Setup(repo => repo.GetAllLitterAsync())
                             .ReturnsAsync(litters);

            // Act
            var result = await _controller.GetAllLitter();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedList = okResult.Value as List<Litter>;
            Assert.IsNotNull(returnedList);
            Assert.AreEqual(2, returnedList.Count);
        }

        [TestMethod]
        public async Task GetAllLitter_LitterIsNull_ReturnsNotFound()
        {
            // Arrange
            _litterRepository.Setup(repo => repo.GetAllLitterAsync())
                             .ReturnsAsync((List<Litter>)null);

            // Act
            var result = await _controller.GetAllLitter();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateLitter_ValidLitter_ReturnsCreated()
        {
            var newLitter = new Litter { Id = Guid.NewGuid().ToString() };

            _litterRepository.Setup(r => r.GetByLitterIdAsync(newLitter.Id)).ReturnsAsync((Litter)null);
            _litterRepository.Setup(r => r.CreateLitterAsync(newLitter)).ReturnsAsync(newLitter);

            var result = await _controller.CreateLitter(newLitter);

            var createdResult = result.Result as CreatedAtRouteResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(newLitter, createdResult.Value);
        }

        [TestMethod]
        public async Task CreateLitter_ExistingLitter_ReturnsBadRequest()
        {
            var existingLitter = new Litter { Id = Guid.NewGuid().ToString() };

            _litterRepository.Setup(r => r.GetByLitterIdAsync(existingLitter.Id)).ReturnsAsync(existingLitter);

            var result = await _controller.CreateLitter(existingLitter);

            var badRequest = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual("Litter with this ID already exists.", badRequest.Value);
        }


        [TestMethod]
        public async Task UpdateLitter_ValidRequest_ReturnsOk()
        {
            var litterId = Guid.NewGuid().ToString();
            var updatedLitter = new Litter { Id = litterId };

            _litterRepository.Setup(r => r.GetByLitterIdAsync(litterId)).ReturnsAsync(updatedLitter);

            var result = await _controller.UpdateLitter(litterId, updatedLitter);

            var ok = result.Result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(updatedLitter, ok.Value);
        }

        [TestMethod]
        public async Task UpdateLitter_LitterNotFound_ReturnsNotFound()
        {
            var litterId = Guid.NewGuid().ToString();
            var updatedLitter = new Litter { Id = litterId };

            _litterRepository.Setup(r => r.GetByLitterIdAsync(litterId)).ReturnsAsync((Litter)null);

            var result = await _controller.UpdateLitter(litterId, updatedLitter);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateLitter_IdMismatch_ReturnsBadRequest()
        {
            var litterId = Guid.NewGuid().ToString();
            var differentId = Guid.NewGuid().ToString();
            var litter = new Litter { Id = differentId };

            var result = await _controller.UpdateLitter(litterId, litter);

            var badRequest = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Litter data is required or ID mismatch.", badRequest.Value);
        }

        [TestMethod]
        public async Task DeleteLitter_Exists_ReturnsNoContent()
        {
            var litterId = Guid.NewGuid().ToString();
            var litter = new Litter { Id = litterId };

            _litterRepository.Setup(r => r.GetByLitterIdAsync(litterId)).ReturnsAsync(litter);
            _litterRepository.Setup(r => r.DeleteLitterAsync(litterId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteLitter(litterId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteLitter_NotFound_ReturnsNotFound()
        {
            var litterId = Guid.NewGuid().ToString();

            _litterRepository.Setup(r => r.GetByLitterIdAsync(litterId)).ReturnsAsync((Litter)null);

            var result = await _controller.DeleteLitter(litterId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
