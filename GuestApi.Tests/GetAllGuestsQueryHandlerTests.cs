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
    public class GetAllGuestsQueryHandlerTests
    {
        private readonly Mock<ILogger<GuestsController>> _loggerMock = new Mock<ILogger<GuestsController>>();

        [Fact]
        public void Handle_ReturnsAllGuests()
        {
            var guests = new List<Guest> 
            { 
                new Guest { Id = Guid.NewGuid(), FirstName = "Abhishek", PhoneNumbers = new List<string> { "1234567890" } },
                new Guest { Id = Guid.NewGuid(), FirstName = "Raghav", PhoneNumbers = new List<string> { "9876543210" } } 
            
            };
            var handler = new GetAllGuestsQueryHandler(guests, Mock.Of<ILogger<GetAllGuestsQueryHandler>>());
            var query = new GetAllGuestsQuery();

            var result = handler.Handle(query);

            Assert.Equal(guests, result);
        }

        [Fact]
        public void Handle_NoGuests_ReturnsEmptyList()
        {
            List<Guest> guests = new List<Guest>() ;
            var loggerMock = new Mock<ILogger<GetAllGuestsQueryHandler>>();
            var handler = new GetAllGuestsQueryHandler(guests, loggerMock.Object);
            var query = new GetAllGuestsQuery();

            var result = handler.Handle(query);
            
            Assert.Empty(result);
        }
    }
}
