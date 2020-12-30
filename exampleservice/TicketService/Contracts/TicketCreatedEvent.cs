using exampleservice.Framework.BaseFramework;

namespace exampleservice.TicketService.Contracts
{
    public class TicketCreatedEvent : EventBase
    {
        public string TicketNumber { get; set; }
    }
}
