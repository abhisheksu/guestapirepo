using GuestApi.Queries;
using GuestApi.Models;
using System.Linq;

namespace GuestApi.Handlers
{
    public class GetGuestByIdQueryHandler: IQueryHandler<GetGuestByIdQuery, Guest>
    {
        private readonly List<Guest> _guests;
        private readonly ILogger<GetGuestByIdQueryHandler> _logger;

        public GetGuestByIdQueryHandler(List<Guest> guests, ILogger<GetGuestByIdQueryHandler> logger)
        {
            _guests = guests;
            _logger = logger;
        }

        public Guest Handle(GetGuestByIdQuery query)
        {
            _logger.LogInformation("Executing GetGuestByIdQueryHandler for guest ID {Id}", query.Id);

            var guest = _guests.FirstOrDefault(g => g.Id == query.Id);

            if (guest == null)
            {
                _logger.LogWarning("Guest with ID {Id} not found.", query.Id);
                throw new InvalidOperationException($"Guest with ID {query.Id} not found.");
            }

            _logger.LogInformation("Returning guest with ID {Id}.", query.Id);
            return guest;
        }
    }
}
