using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smarth_health.WebApi.Controllers;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace Trashtracker_api.test
{
    [TestClass]
    public class AuthControllerTests
    {
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<IUserRepository> _mockUserRepo;
        private AuthController _controller;

        [TestInitialize]
        public void Setup()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _mockUserRepo = new Mock<IUserRepository>();
            _controller = new AuthController(_mockUserManager.Object, _mockUserRepo.Object);
        }

        [TestMethod]
        public async Task Register_NullUser_ReturnsBadRequest()
        {
            var result = await _controller.Register(null);
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("User data is required", badRequest.Value);
        }

        [TestMethod]
        public async Task Register_UserAlreadyExists_ReturnsBadRequest()
        {
            var user = new User { Username = "existinguser" };
            _mockUserRepo.Setup(r => r.GetUserAsync(user.Username)).ReturnsAsync(user);

            var result = await _controller.Register(user);
            var badRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequest);
            Assert.AreEqual("User is already registered", badRequest.Value);
        }

        [TestMethod]
        public async Task Register_CreateIdentityUserFails_ReturnsBadRequest()
        {
            var user = new User { Username = "testuser", Email = "test@example.com", Password = "Test123!" };
            _mockUserRepo.Setup(r => r.GetUserAsync(user.Username)).ReturnsAsync((User)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), user.Password))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed" }));

            var result = await _controller.Register(user);
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
        }

        [TestMethod]
        public async Task Register_CreateNonIdentityUserFails_ReturnsBadRequest()
        {
            var user = new User { Username = "testuser", Email = "test@example.com", Password = "Test123!" };
            _mockUserRepo.Setup(r => r.GetUserAsync(user.Username)).ReturnsAsync((User)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), user.Password))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).ReturnsAsync((User)null);

            var result = await _controller.Register(user);
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Failed to create (non-identity user)", badRequest.Value);
        }

        [TestMethod]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var user = new User { Username = "testuser", Email = "test@example.com", Password = "Test123!" };

            _mockUserRepo.Setup(r => r.GetUserAsync(user.Username)).ReturnsAsync((User)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), user.Password))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

            var result = await _controller.Register(user);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.IsInstanceOfType(ok.Value, typeof(string));
        }
    }
}
