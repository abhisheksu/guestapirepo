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
    public class AddPhoneCommandHandlerTests
    {
        [Fact]
        public void Handle_ValidCommand_AddsPhoneToGuest()
        {
            var guests = new List<Guest> { new Guest { Id = Guid.NewGuid() } };
            var handler = new AddPhoneCommandHandler(guests, Mock.Of<ILogger<AddPhoneCommandHandler>>());
            var command = new AddPhoneCommand { GuestId = guests[0].Id, PhoneNumber = "1234567890" };

            handler.Handle(command);

            Assert.Single(guests[0].PhoneNumbers);
            Assert.Equal("1234567890", guests[0].PhoneNumbers[0]);
        }

        [Fact]
        public void Handle_InvalidCommand_ThrowsException()
        {
            var guests = new List<Guest>();
            var loggerMock = new Mock<ILogger<AddPhoneCommandHandler>>();
            var handler = new AddPhoneCommandHandler(guests, loggerMock.Object);
            var command = new AddPhoneCommand { GuestId = Guid.NewGuid(), PhoneNumber = "" };

            Assert.Throws<ArgumentException>(() => handler.Handle(command));
        }

        [Fact]
        public void Handle_NonExistingGuest_ThrowsException()
        {
            var guests = new List<Guest>();
            var guestId = Guid.NewGuid();
            var loggerMock = new Mock<ILogger<AddPhoneCommandHandler>>();
            var handler = new AddPhoneCommandHandler(guests, loggerMock.Object);
            var command = new AddPhoneCommand { GuestId = guestId, PhoneNumber = "1234567890" };
            
            var exception = Assert.Throws<InvalidOperationException>(() => handler.Handle(command));
            Assert.Equal("Guest not found.", exception.Message);
        }

        [Fact]
        public void Handle_ExistingPhoneNumber_ThrowsException()
        {
            var guestId = Guid.NewGuid();
            var guests = new List<Guest>
            {
                new Guest { Id = guestId, FirstName = "Abhishek", PhoneNumbers = new List<string> { "1234567890" } }
            };
            var loggerMock = new Mock<ILogger<AddPhoneCommandHandler>>();
            var handler = new AddPhoneCommandHandler(guests, loggerMock.Object);
            var command = new AddPhoneCommand { GuestId = guestId, PhoneNumber = "1234567890" };

            var exception = Assert.Throws<ArgumentException>(() => handler.Handle(command));
            Assert.Equal("Phone number already exists.", exception.Message);
        }
    }

}
