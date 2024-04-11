using GuestApi.Queries;
using GuestApi.Models;
using System.Linq;

namespace GuestApi.Handlers
{
    public class GetAllGuestsQueryHandler: IQueryHandler<GetAllGuestsQuery, List<Guest>>
    {
        private readonly List<Guest> _guests;
        private readonly ILogger<GetAllGuestsQueryHandler> _logger;

        public GetAllGuestsQueryHandler(List<Guest> guests, ILogger<GetAllGuestsQueryHandler> logger)
        {
            _guests = guests;
            _logger = logger;
        }

        public List<Guest> Handle(GetAllGuestsQuery query)
        {
           _logger.LogInformation("Executing GetAllGuestsQueryHandler");
            
            if (_guests == null)
            {
                _logger.LogWarning("No guests found.");
                return new List<Guest>();
            }

            _logger.LogInformation("Returning all guests.");
            return _guests;
        }
    }
}
