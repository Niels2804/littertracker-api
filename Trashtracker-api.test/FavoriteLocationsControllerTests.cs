using Microsoft.AspNetCore.Mvc;
using Moq;
using trashtracker_api.Controllers;
using trashtracker_api.Models;
using trashtracker_api.Repositories.Interfaces;
namespace Trashtracker_api.test
{
    [TestClass]
    public class FavoriteLocationsControllerTests
    {
        private Mock<IFavoriteLocationsRepository> _favoriteLocationsRepoMock;
        private FavoriteLocationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _favoriteLocationsRepoMock = new Mock<IFavoriteLocationsRepository>();
            _controller = new FavoriteLocationsController(_favoriteLocationsRepoMock.Object);
        }

        [TestMethod]
        public async Task CreateFavoriteLocation_NewLocation_ReturnsCreatedAtRoute()
        {
            // Arrange
            var favoriteLocation = new FavoriteLocation
            {
                UserId = "user123",
                LitterId = "litter456",
                Rating = 1
            };

            _favoriteLocationsRepoMock
                .Setup(r => r.GetFavoriteLocationsAsync(favoriteLocation.UserId, favoriteLocation.LitterId))
                .ReturnsAsync((FavoriteLocation)null);

            _favoriteLocationsRepoMock
                .Setup(r => r.CreateFavoriteLocationAsync(It.IsAny<FavoriteLocation>()))
                .ReturnsAsync(favoriteLocation);

            // Act
            var result = await _controller.CreateFavoriteLocation(favoriteLocation);

            // Assert
            var createdResult = result as CreatedAtRouteResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(FavoriteLocationsController.GetFavoriteLocationsByUserId), createdResult.RouteName);
            Assert.AreEqual(favoriteLocation.UserId, createdResult.RouteValues["identityUserId"]);
        }

        [TestMethod]
        public async Task CreateFavoriteLocation_ExistingLocation_IncrementsRating()
        {
            // Arrange
            var existing = new FavoriteLocation
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "user123",
                LitterId = "litter456",
                Rating = 2
            };

            _favoriteLocationsRepoMock
                .Setup(r => r.GetFavoriteLocationsAsync(existing.UserId, existing.LitterId))
                .ReturnsAsync(existing);

            // Act
            var result = await _controller.CreateFavoriteLocation(existing);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Favorite location rating increased", okResult.Value);

            _favoriteLocationsRepoMock.Verify(r => r.UpdateFavoriteLocationAsync(existing.Id, It.Is<FavoriteLocation>(f => f.Rating == 3)), Times.Once);
        }

        [TestMethod]
        public async Task CreateFavoriteLocation_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalid = new FavoriteLocation
            {
                UserId = "",
                LitterId = "",
                Rating = -1
            };

            // Act
            var result = await _controller.CreateFavoriteLocation(invalid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetFavoriteLocationsByUserId_UserHasFavorites_ReturnsOk()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            var locations = new List<FavoriteLocation>
        {
            new FavoriteLocation { Id = "1", UserId = userId, LitterId = "abc", Rating = 2 }
        };

            _favoriteLocationsRepoMock
                .Setup(r => r.GetAllFavoriteLocationsAsync(userId))
                .ReturnsAsync(locations);

            // Act
            var result = await _controller.GetFavoriteLocationsByUserId(userId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returned = okResult.Value as IEnumerable<FavoriteLocation>;
            Assert.IsNotNull(returned);
            Assert.AreEqual(1, returned.Count());
        }

        [TestMethod]
        public async Task GetFavoriteLocationsByUserId_UserHasNoFavorites_ReturnsNotFound()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            _favoriteLocationsRepoMock
                .Setup(r => r.GetAllFavoriteLocationsAsync(userId))
                .ReturnsAsync(new List<FavoriteLocation>());

            // Act
            var result = await _controller.GetFavoriteLocationsByUserId(userId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task UpdateFavoriteLocation_ValidUpdate_ReturnsOk()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var updated = new FavoriteLocation
            {
                Id = id,
                UserId = "user123",
                LitterId = "lit789",
                Rating = 3
            };

            _favoriteLocationsRepoMock
                .Setup(r => r.GetFavoriteLocationsAsync(id))
                .ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateFavoriteLocationsById(id, updated);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _favoriteLocationsRepoMock.Verify(r => r.UpdateFavoriteLocationAsync(id, updated), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFavoriteLocation_NotFound_ReturnsNotFound()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var updated = new FavoriteLocation
            {
                Id = id,
                UserId = "user123",
                LitterId = "lit789",
                Rating = 3
            };

            _favoriteLocationsRepoMock
                .Setup(r => r.GetFavoriteLocationsAsync(id))
                .ReturnsAsync((FavoriteLocation)null);

            // Act
            var result = await _controller.UpdateFavoriteLocationsById(id, updated);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteAllFavoriteLocation_LocationsExist_ReturnsNoContent()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            var favoriteLocations = new List<FavoriteLocation>
    {
        new FavoriteLocation { Id = Guid.NewGuid().ToString(), UserId = userId, LitterId = "123", Rating = 1 }
    };

            _favoriteLocationsRepoMock.Setup(r => r.GetAllFavoriteLocationsAsync(userId))
                                       .ReturnsAsync(favoriteLocations);

            _favoriteLocationsRepoMock.Setup(r => r.DeleteAllFavoriteLocationsAsync(userId))
                                       .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAllFavoriteLocation(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteAllFavoriteLocation_NoLocationsFound_ReturnsNotFound()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            _favoriteLocationsRepoMock.Setup(r => r.GetAllFavoriteLocationsAsync(userId))
                                       .ReturnsAsync(new List<FavoriteLocation>());

            // Act
            var result = await _controller.DeleteAllFavoriteLocation(userId);

            // Assert
            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual("No favorite locations found for the user", notFound.Value);
        }
    }
}
