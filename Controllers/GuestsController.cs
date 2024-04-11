using GuestApi.Commands;
using GuestApi.Queries;
using GuestApi.Models;
using GuestApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GuestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        private readonly ILogger<GuestsController> _logger;
        
        private readonly ICommandHandler<AddGuestCommand> _addGuestCommandHandler;
        private readonly ICommandHandler<AddPhoneCommand> _addPhoneCommandHandler;
        private readonly IQueryHandler<GetGuestByIdQuery, Guest> _getGuestByIdQueryHandler;
        private readonly IQueryHandler<GetAllGuestsQuery, List<Guest>> _getAllGuestsQueryHandler;

        public GuestsController(ILogger<GuestsController> logger, 
                                ICommandHandler<AddGuestCommand> addGuestCommandHandler,
                                ICommandHandler<AddPhoneCommand> addPhoneCommandHandler,
                                IQueryHandler<GetGuestByIdQuery, Guest> getGuestByIdQueryHandler,
                                IQueryHandler<GetAllGuestsQuery, List<Guest>> getAllGuestsQueryHandler)
        {
            _logger = logger;
            _addGuestCommandHandler = addGuestCommandHandler;
            _addPhoneCommandHandler = addPhoneCommandHandler;
            _getGuestByIdQueryHandler = getGuestByIdQueryHandler;
            _getAllGuestsQueryHandler = getAllGuestsQueryHandler;
        }

        [HttpPost]
        public IActionResult AddGuest(AddGuestCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _addGuestCommandHandler.Handle(command);
                _logger.LogInformation("Guest added successfully.");
                return Created("", command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("{guestId}/phones")]
        public IActionResult AddPhone(Guid guestId, AddPhoneCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingPhoneNumbers = _getAllGuestsQueryHandler.Handle(new GetAllGuestsQuery())
                                                .SelectMany(g => g.PhoneNumbers)
                                                .ToList();

                if (existingPhoneNumbers.Contains(command.PhoneNumber))
                {
                    return BadRequest("Phone number already exists.");
                }

                command.GuestId = guestId;
                _addPhoneCommandHandler.Handle(command);
                _logger.LogInformation("Phone added successfully.");
                return Created("", command.PhoneNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating phone number.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }   

        [HttpGet("{id}")]
        public IActionResult GetGuestById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Invalid guest ID.");
                }
                var query = new GetGuestByIdQuery { Id = id };
                var guest = _getGuestByIdQueryHandler.Handle(query);
                if (guest != null)
                {
                    _logger.LogInformation("Guest found.");
                    return Ok(guest);
                }
                else
                {
                    _logger.LogInformation("Guest not found.");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the guest.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public IActionResult GetAllGuests()
        {
            try
            {
                var query = new GetAllGuestsQuery();
                var guests = _getAllGuestsQueryHandler.Handle(query);
                _logger.LogInformation("Retrieved all guests.");
                return Ok(guests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all guests.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
