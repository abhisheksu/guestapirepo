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
    public class GuestsControllerTests
    {
        private readonly GuestsController _controller;
        private readonly Mock<ICommandHandler<AddGuestCommand>> _addGuestCommandHandlerMock = new Mock<ICommandHandler<AddGuestCommand>>();
        private readonly Mock<ICommandHandler<AddPhoneCommand>> _addPhoneCommandHandlerMock = new Mock<ICommandHandler<AddPhoneCommand>>();
        private readonly Mock<IQueryHandler<GetGuestByIdQuery, Guest>> _getGuestByIdQueryHandlerMock = new Mock<IQueryHandler<GetGuestByIdQuery, Guest>>();
        private readonly Mock<IQueryHandler<GetAllGuestsQuery, List<Guest>>> _getAllGuestsQueryHandlerMock = new Mock<IQueryHandler<GetAllGuestsQuery, List<Guest>>>();
        private readonly Mock<ILogger<GuestsController>> _loggerMock = new Mock<ILogger<GuestsController>>();

        public GuestsControllerTests()
        {
            _controller = new GuestsController(
                _loggerMock.Object,
                _addGuestCommandHandlerMock.Object,
                _addPhoneCommandHandlerMock.Object,
                _getGuestByIdQueryHandlerMock.Object,
                _getAllGuestsQueryHandlerMock.Object);
        }

        [Fact]
        public void AddGuest_ValidGuest_ReturnsCreatedResult()
        {            
            var command = new AddGuestCommand();
            _addGuestCommandHandlerMock.Setup(m => m.Handle(command));
            
            var result = _controller.AddGuest(command);
            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public void AddGuest_InvalidGuest_ReturnsBadRequest()
        {            
            var command = new AddGuestCommand();
            _controller.ModelState.AddModelError("FirstName", "First name is required.");
            
            var result = _controller.AddGuest(command);
            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddGuest_ExceptionThrown_ReturnsStatusCode500()
        {            
            var command = new AddGuestCommand();
            _addGuestCommandHandlerMock.Setup(m => m.Handle(command)).Throws(new Exception());
            
            var result = _controller.AddGuest(command);
            
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void AddPhone_InvalidModelState_ReturnsBadRequest()
        {            
            _controller.ModelState.AddModelError("error", "some error");
            
            var result = _controller.AddPhone(Guid.NewGuid(), new AddPhoneCommand());
            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddPhone_PhoneNumberAlreadyExists_ReturnsBadRequest()
        {            
            var existingGuest = new Guest { Id = Guid.NewGuid(), PhoneNumbers = new List<string> { "1234567890" } };
            _getAllGuestsQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetAllGuestsQuery>()))
                .Returns(new List<Guest> { existingGuest });
            
            var result = _controller.AddPhone(existingGuest.Id, new AddPhoneCommand { PhoneNumber = "1234567890" });
            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddPhone_ValidRequest_ReturnsCreatedResult()
        {            
            var guestId = Guid.NewGuid();
            var phoneNumber = "1234567890";
            var command = new AddPhoneCommand { GuestId = guestId, PhoneNumber = phoneNumber };
            var guest = new Guest { Id = guestId, PhoneNumbers = new List<string>() };
            _getAllGuestsQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetAllGuestsQuery>()))
                .Returns(new List<Guest> { guest });
            
            var result = _controller.AddPhone(guestId, command);
            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public void GetGuestById_ExistingId_ReturnsOkResultWithGuest()
        {            
            var guestId = Guid.NewGuid();
            var query = new GetGuestByIdQuery { Id = guestId };
            var guest = new Guest { Id = guestId };
            _getGuestByIdQueryHandlerMock.Setup(m => m.Handle(query)).Returns(guest);
            
            var result = _controller.GetGuestById(guestId);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var returnedGuest = Assert.IsType<Guest>(okObjectResult.Value);
            Assert.NotNull(returnedGuest);
            Assert.Equal(guestId, returnedGuest.Id);
        }

        [Fact]
        public void GetGuestById_NonExistingId_ReturnsNotFoundResult()
        {            
            var guestId = Guid.NewGuid();
            var query = new GetGuestByIdQuery { Id = guestId };
            _getGuestByIdQueryHandlerMock.Setup(m => m.Handle(query)).Returns((Guest)null);
            
            var result = _controller.GetGuestById(guestId);
            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetGuestById_ExceptionThrown_ReturnsStatusCode500()
        {            
            var id = Guid.NewGuid();
            _getGuestByIdQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetGuestByIdQuery>())).Throws(new Exception());
            
            var result = _controller.GetGuestById(id);
            
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, ((StatusCodeResult)result).StatusCode);
        }


        [Fact]
        public void GetAllGuests_ReturnsOkResultWithGuestList()
        {            
            var guests = new List<Guest>
            {
                new Guest { Id = Guid.NewGuid(), FirstName = "John" },
                new Guest { Id = Guid.NewGuid(), FirstName = "Jane" }
            };
            _getAllGuestsQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetAllGuestsQuery>())).Returns(guests);
            
            var result = _controller.GetAllGuests();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var returnedGuests = Assert.IsType<List<Guest>>(okObjectResult.Value);
            Assert.NotNull(returnedGuests);
            Assert.Equal(guests.Count, returnedGuests.Count);
            Assert.Equal(guests[0].FirstName, returnedGuests[0].FirstName);
            Assert.Equal(guests[1].FirstName, returnedGuests[1].FirstName);
        }

        [Fact]
        public void GetAllGuests_EmptyList_ReturnsEmptyList()
        {            
            var guests = new List<Guest>();
            _getAllGuestsQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetAllGuestsQuery>())).Returns(guests);
            
            var result = _controller.GetAllGuests();
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Guest>>(okObjectResult.Value);
            Assert.Empty(model);
        }

        [Fact]
        public void GetAllGuests_ExceptionThrown_ReturnsStatusCode500()
        {            
            _getAllGuestsQueryHandlerMock.Setup(m => m.Handle(It.IsAny<GetAllGuestsQuery>())).Throws(new Exception());
            
            var result = _controller.GetAllGuests();
            
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, ((StatusCodeResult)result).StatusCode);
        }
    }
}
