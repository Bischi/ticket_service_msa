using exampleservice.TicketService.Contracts;

namespace exampleservice.TicketService
{
    public class FlagTicketAsSoldContext
    {
        public bool WasCompensated { get; set; }

        public FlagTicketAsSoldCommand Command { get; set; }
    }
}
