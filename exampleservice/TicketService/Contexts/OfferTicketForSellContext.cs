using exampleservice.TicketService.Contracts;

namespace exampleservice.TicketService
{
    public class OfferTicketForSellContext
    {
        public bool WasCompensated { get; set; }

        public OfferTicketForSellCommand Command { get; set; }
    }
}
