﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Moq;

using Ninject;
using Ninject.MockingKernel;

using RememBeer.Models.Contracts;
using RememBeer.MvcClient.Hubs;
using RememBeer.Services.Contracts;
using RememBeer.Tests.MvcClient.Controllers.Ninject.Base;
// ReSharper disable InconsistentNaming

namespace RememBeer.Tests.MvcClient.Hubs.NotificationsHubTests.Base
{
    public interface IDynamicNotified
    {
        void onFollowerReviewCreated(int id, string username);

        void showNotification(string message, string username, string lat, string lon);
    }

    public class NotificationsHubNinjectTestBase : NinjectTestBase
    {
        public const string LoggedInContextName = "LoggedIn";

        public override void Init()
        {
            this.MockingKernel.Bind<IDynamicNotified>().ToMock().InSingletonScope();
            this.MockingKernel.Bind<IHubCallerConnectionContext<object>>().ToMock().InSingletonScope();
            this.MockingKernel.Bind<IRequest>().ToMock().InSingletonScope();
            this.MockingKernel.Bind<IFollowerService>().ToMock().InSingletonScope();
            this.MockingKernel.Bind<IBeerReviewService>().ToMock().InSingletonScope();

            this.MockingKernel.Bind<NotificationsHub>()
                .ToSelf()
                .Named(LoggedInContextName)
                .BindingConfiguration.IsImplicit = true;

            this.MockingKernel.Bind<NotificationsHub>()
                .ToMethod(ctx =>
                          {
                              var sut = this.MockingKernel.Get<NotificationsHub>(LoggedInContextName);

                              var mockDynamic = this.MockingKernel.GetMock<IDynamicNotified>();
                              var clients = this.MockingKernel.GetMock<IHubCallerConnectionContext<object>>();
                              clients.Setup(c => c.Users(It.IsAny<IList<string>>()))
                                     .Returns(mockDynamic.Object);
                              sut.Clients = clients.Object;

                              var context = this.MockingKernel.GetMock<HubCallerContext>();
                              sut.Context = context.Object;

                              return sut;
                          });

            this.MockingKernel.Bind<HubCallerContext>()
                .ToMethod(ctx =>
                          {
                              var identity = this.MockingKernel.Get<ClaimsIdentity>();
                              var context = new Mock<HubCallerContext>();
                              context.SetupGet(c => c.User.Identity).Returns(identity);

                              return context.Object;
                          })
                .InSingletonScope();
        }

        protected IEnumerable<IApplicationUser> GetMockedUsers(int count)
        {
            var result = new List<IApplicationUser>();
            for (var i = 0; i < count; i++)
            {
                var user = new Mock<IApplicationUser>();
                user.Setup(u => u.Id)
                    .Returns(Guid.NewGuid().ToString());
                result.Add(user.Object);
            }

            return result;
        }
    }
}