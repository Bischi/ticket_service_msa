// CreateTicketCommand.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Contracts
{
    public class CreateTicketCommand
    {
        public Ticket Ticket { get; set; }
    }
}
