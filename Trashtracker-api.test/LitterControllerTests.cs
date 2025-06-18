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
    }
}
