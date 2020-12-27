// GetTicketsCommand.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
namespace exampleservice.TicketService.Contracts
{
    public class GetTicketsCommand
    {
        public bool OnlySoldTickets { get; set; }
    }
}
