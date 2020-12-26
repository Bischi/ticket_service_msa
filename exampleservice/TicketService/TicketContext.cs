// TicketServiceContext.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using exampleservice.TicketService.Contracts;

namespace exampleservice.TicketService
{
    public class TicketContext
    {
        public bool WasCompensated { get; set; }

        public CreateTicketCommand Command { get; set; }
    }
}
