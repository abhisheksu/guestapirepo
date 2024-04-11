using GuestApi.Commands;
using GuestApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace GuestApi.Handlers
{
    public class AddPhoneCommandHandler: ICommandHandler<AddPhoneCommand>
    {
        private readonly List<Guest> _guests;
        private readonly ILogger<AddPhoneCommandHandler> _logger;

        public AddPhoneCommandHandler(List<Guest> guests,ILogger<AddPhoneCommandHandler> logger)
        {
            _guests = guests;
            _logger = logger;
        }

        public void Handle(AddPhoneCommand command)
        {
            _logger.LogInformation("Executing AddPhoneCommandHandler for guest ID {GuestId}", command.GuestId);

            try
            {
                var guest = _guests.FirstOrDefault(g => g.Id == command.GuestId);

                if (guest == null)
                {
                    _logger.LogWarning("Guest with ID {GuestId} not found.", command.GuestId);
                    throw new InvalidOperationException($"Guest with ID {command.GuestId} not found.");
                }

                if (string.IsNullOrWhiteSpace(command.PhoneNumber))
                {
                    _logger.LogWarning("Phone number is required.");
                    throw new ArgumentException("Phone number is required.", nameof(command.PhoneNumber));
                }
                var existingPhoneNumbers = _guests.SelectMany(g => g.PhoneNumbers).ToList();
                if (existingPhoneNumbers.Contains(command.PhoneNumber))
                {
                    _logger.LogWarning("Phone number {PhoneNumber} already exists.", command.PhoneNumber);
                    throw new ArgumentException("Phone number already exists.", nameof(command.PhoneNumber));
                }
                
                guest.PhoneNumbers.Add(command.PhoneNumber);
                _logger.LogInformation("Phone number {PhoneNumber} added successfully for guest ID {GuestId}.", command.PhoneNumber, command.GuestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing AddPhoneCommand");
                throw;
            }
            
        }
    }
}
