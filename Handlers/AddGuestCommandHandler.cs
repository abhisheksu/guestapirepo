using GuestApi.Commands;
using GuestApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace GuestApi.Handlers
{
    public class AddGuestCommandHandler: ICommandHandler<AddGuestCommand>
    {
        private readonly List<Guest> _guests;
        private readonly ILogger<AddGuestCommandHandler> _logger;

        public AddGuestCommandHandler(List<Guest> guests,ILogger<AddGuestCommandHandler> logger)
        {
            _guests = guests;
            _logger = logger;
        }

        public void Handle(AddGuestCommand command)
        {
            _logger.LogInformation("Executing AddGuestCommandHandler for guest ID {GuestId}", command.Id);
            try
            {
                if (string.IsNullOrWhiteSpace(command.FirstName))
                {
                     _logger.LogWarning("First name is required.");
                    throw new ArgumentException("First name is required.", nameof(command.FirstName));
                }

                if (command.PhoneNumbers == null || !command.PhoneNumbers.Any())
                {
                    _logger.LogWarning("At least one phone number is required.");
                    throw new ArgumentException("At least one phone number is required.", nameof(command.PhoneNumbers));
                }
            
                var guest = new Guest
                    {
                        Id = command.Id,
                        Title = command.Title,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        BirthDate = command.BirthDate,
                        Email = command.Email,
                        PhoneNumbers = command.PhoneNumbers
                    };

                _guests.Add(guest);
                _logger.LogInformation("Guest {GuestId} added successfully.", command.Id);
    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing AddGuestCommand.");
                throw;
            }
        }
    }
}
