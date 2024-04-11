using GuestApi.Commands;
using GuestApi.Controllers;
using GuestApi.Handlers;
using GuestApi.Models;
using GuestApi.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GuestApi.Tests
{
        public class GetGuestByIdQueryHandlerTests
    {
        [Fact]
        public void Handle_ExistingId_ReturnsGuest()
        {
            var guests = new List<Guest> { new Guest { Id = Guid.NewGuid(), FirstName = "Abhishek" } };
            var handler = new GetGuestByIdQueryHandler(guests, Mock.Of<ILogger<GetGuestByIdQueryHandler>>());
            var query = new GetGuestByIdQuery { Id = guests[0].Id };

            var result = handler.Handle(query);

            Assert.NotNull(result);
            Assert.Equal("Abhishek", result.FirstName);
        }
    }

}
