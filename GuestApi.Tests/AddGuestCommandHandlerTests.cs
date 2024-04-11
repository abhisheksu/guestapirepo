using Xunit;
using Moq;
using GuestApi.Commands;
using GuestApi.Models;
using GuestApi.Handlers;
using Microsoft.Extensions.Logging;
using System;

namespace GuestApi.Tests
{
    public class AddGuestCommandHandlerTests
    {
        [Fact]
        public void Handle_ValidCommand_AddsGuestToList()
        {           
            var guests = new List<Guest>();
            var loggerMock = new Mock<ILogger<AddGuestCommandHandler>>();
            var handler = new AddGuestCommandHandler(guests, loggerMock.Object);

            var command = new AddGuestCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "Abhishek",
                LastName = "U",
                PhoneNumbers = new List<string> { "1234567890" }
            };
            
            handler.Handle(command);
            
            Assert.Single(guests);
            Assert.Equal(command.Id, guests[0].Id);
            Assert.Equal(command.FirstName, guests[0].FirstName);
            Assert.Equal(command.LastName, guests[0].LastName);
            Assert.Equal(command.PhoneNumbers, guests[0].PhoneNumbers);
        }

        [Fact]
        public void Handle_InvalidCommand_ThrowsArgumentException()
        {           
            var guests = new List<Guest>();
            var loggerMock = new Mock<ILogger<AddGuestCommandHandler>>();
            var handler = new AddGuestCommandHandler(guests, loggerMock.Object);

            var command = new AddGuestCommand(); // Missing required fields
             
            Assert.Throws<ArgumentException>(() => handler.Handle(command));
        }

        [Fact]
        public void Handle_Exception_LogsError()
        {           
            var guests = new List<Guest>();
            var loggerMock = new Mock<ILogger<AddGuestCommandHandler>>();
            var handler = new AddGuestCommandHandler(guests, loggerMock.Object);

            var command = new AddGuestCommand
            {
                Id = Guid.NewGuid(),
                FirstName = "Abhishek",
                LastName = "U",
                PhoneNumbers = new List<string> { "1234567890" }
            };

            loggerMock.Setup(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
            
            Assert.Throws<Exception>(() => handler.Handle(command));
            
            loggerMock.Verify(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }
    }
}