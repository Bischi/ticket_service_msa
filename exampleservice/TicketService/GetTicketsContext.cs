// TicketServiceContext.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Collections.Generic;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService
{
    public class GetTicketsContext
    {
        public GetTicketsCommand Command { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
