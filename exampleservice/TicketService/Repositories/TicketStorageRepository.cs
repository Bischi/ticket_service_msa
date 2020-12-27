// TicketStoreRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System.Collections.Generic;
using System.Threading.Tasks;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Repositories
{
    public class TicketStorageRepository : ITicketStorageRepository
    {
        // Id, Ticket
        private Dictionary<string, Ticket> Tickets;

        public TicketStorageRepository()
        {
            Tickets = new Dictionary<string, Ticket>();
        }

        public async Task<int> Add(Ticket ticket)
        {
            return await Task.Run(() =>
            {
                Tickets.Add(ticket.TicketNumber, ticket);
                return 1;
            });
        }

        public async Task<Ticket> Get(string ticketNumber)
        {
            if (Tickets.ContainsKey(ticketNumber))
            {
                return await Task.Run(() =>
                {
                    return Tickets[ticketNumber];
                });
            }
            else return null;
        }

        public async Task<int> Save(Ticket ticket)
        {
            if (Tickets.ContainsKey(ticket.TicketNumber))
            {
                return await Task.Run(() =>
                {
                    Tickets[ticket.TicketNumber] = ticket;
                    return 1;
                });
            }
            else return 0;
        }
    }
}
