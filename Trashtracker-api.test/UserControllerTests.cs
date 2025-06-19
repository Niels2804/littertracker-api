using Microsoft.AspNetCore.Mvc;
using Moq;
using PasswordHasher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trashtracker_api.Controllers;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace Trashtracker_api.test
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _controller = new UserController(_mockUserRepo.Object);
        }

        [TestMethod]
        public async Task CreateUser_ValidUser_ReturnsCreated()
        {
            // Arrange
            var user = new User { Username = "test", Password = "password" };
            _mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>()))
                         .ReturnsAsync(user);

            // Act
            var result = await _controller.CreateUser(user);

            // Assert
            var createdAt = result as CreatedAtRouteResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual("ReadUserByUsername", createdAt.RouteName);
        }

        [TestMethod]
        public async Task CreateUser_NullUser_ReturnsBadRequest()
        {
            var result = await _controller.CreateUser(null);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task VerifyUser_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var plainPassword = "password";
            var hashedPassword = PasswordHelper.HashPassword(plainPassword);
            var user = new User { Username = "test", Password = plainPassword };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync("test"))
                         .ReturnsAsync(new User { Username = "test", Password = hashedPassword });

            // Act
            var result = await _controller.VerifyUser(user);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(true, okResult.Value);
        }

        [TestMethod]
        public async Task VerifyUser_InvalidCredentials_ReturnsUnauthorized()
        {
            var user = new User { Username = "test", Password = "wrongpass" };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync("test"))
                         .ReturnsAsync(new User { Username = "test", Password = PasswordHelper.HashPassword("correctpass") });

            var result = await _controller.VerifyUser(user);

            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task GetUserById_UserExists_ReturnsUser()
        {
            var userId = Guid.NewGuid().ToString();
            var user = new User { IdentityUserId = userId.ToString(), Username = "test" };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var result = await _controller.GetUserById(userId);
            var ok = result.Result as OkObjectResult;

            Assert.IsNotNull(ok);
            Assert.AreEqual(user, ok.Value);
        }

        [TestMethod]
        public async Task GetUserById_NotFound_ReturnsNotFound()
        {
            var userId = Guid.NewGuid().ToString();

            _mockUserRepo.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _controller.GetUserById(userId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetAuthenticationIdByEmail_Valid_ReturnsId()
        {
            var email = "test@example.com";
            _mockUserRepo.Setup(r => r.GetAuthenticationIdByEmailAsync(email))
                         .ReturnsAsync("authId");

            var result = await _controller.GetAuthenticationIdByEmail(email);
            var ok = result.Result as OkObjectResult;

            Assert.IsNotNull(ok);
            Assert.AreEqual("authId", ok.Value);
        }

        [TestMethod]
        public async Task UpdateUser_UserExists_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString(); // string ID
            var user = new User
            {
                IdentityUserId = userId,
                Username = "test"
            };

            _mockUserRepo.Setup(r => r.GetUserByIdAsync(userId))
                         .ReturnsAsync(user);

            _mockUserRepo.Setup(r => r.UpdateUserAsync(user))
                         .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(user);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(user, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            var result = await _controller.Update("authId");
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
