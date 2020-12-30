// TicketsLoadedEvent.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Collections.Generic;
using exampleservice.Framework.BaseFramework;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Contracts
{
    public class TicketsLoadedEvent : EventBase
    {
        public List<Ticket> Tickets { get; set; }
    }
}
