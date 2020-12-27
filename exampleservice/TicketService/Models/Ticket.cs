// Ticket.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
namespace exampleservice.TicketService.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string TicketNumber { get; set; }

        public int FromLocationId { get; set; }

        public int ToLocationId { get; set; }

        public string MeansOfTransport { get; set; }

        public bool isAvailable { get; set; } = true;

        public bool hasOffer { get; set; } = false;
    }
}
