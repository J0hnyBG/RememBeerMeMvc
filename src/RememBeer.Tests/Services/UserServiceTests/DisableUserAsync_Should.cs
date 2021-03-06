﻿using System;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

using Moq;

using NUnit.Framework;

using Ploeh.AutoFixture;

using RememBeer.Models.Factories;
using RememBeer.Models.Identity.Contracts;
using RememBeer.Services;
using RememBeer.Tests.Utils;

namespace RememBeer.Tests.Services.UserServiceTests
{
    public class DisableUserAsync_Should : TestClassBase
    {
        [Test]
        public async Task Call_UserManagerUpdateSecurityStampAsyncMethodOnceWithCorrectParams()
        {
            // Arrange
            var expectedId = this.Fixture.Create<string>();
            var userManager = new Mock<IApplicationUserManager>();
            userManager.Setup(m => m.UpdateSecurityStampAsync(expectedId))
                       .Returns(Task.FromResult(IdentityResult.Failed()));

            var signInManager = new Mock<IApplicationSignInManager>();
            var modelFactory = new Mock<IModelFactory>();

            var service = new UserService(userManager.Object,
                                          signInManager.Object,
                                          modelFactory.Object);

            // Act
            await service.DisableUserAsync(expectedId);

            // Assert
            userManager.Verify(m => m.UpdateSecurityStampAsync(expectedId), Times.Once);
        }

        [Test]
        public async Task Call_UserManagerSetLockoutEndDateAsyncMethodOnceWithCorrectParams()
        {
            // Arrange
            var expectedResult = IdentityResult.Success;
            var expectedId = this.Fixture.Create<string>();
            var userManager = new Mock<IApplicationUserManager>();
            userManager.Setup(m => m.UpdateSecurityStampAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult(expectedResult));

            var signInManager = new Mock<IApplicationSignInManager>();
            var modelFactory = new Mock<IModelFactory>();

            var service = new UserService(userManager.Object,
                                          signInManager.Object,
                                          modelFactory.Object);

            // Act
            await service.DisableUserAsync(expectedId);

            userManager.Verify(m => m.SetLockoutEndDateAsync(expectedId, DateTimeOffset.MaxValue), Times.Once);
        }
    }
}
