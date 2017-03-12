﻿using NUnit.Framework;

using Ploeh.AutoFixture;

using RememBeer.MvcClient.Models.AccountModels;
using RememBeer.Tests.Utils;

namespace RememBeer.Tests.Mvc.Models.Account
{
    [TestFixture]
    public class ForgotPasswordViewModelTests : TestClassBase
    {
        [Test]
        public void Setters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expected = this.Fixture.Create<string>();
            var sut = new ForgotPasswordViewModel();
            // Act
            sut.Email = expected;
            // Assert
            Assert.AreSame(expected, sut.Email);
        }
    }
}