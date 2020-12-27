using System;
using System.Collections.Generic;
using System.Text;

namespace exampleservice.TicketService.Contracts
{
    public class FlagTicketAsSoldCommand
    {
        // Comment Ulrich Gram: As a C# foreigner, I removed the internal set because I couldn't find a way to set a ticket number in the tests
        public string TicketNumber { get; set; }
    }
}
