// TicketStoreRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//

using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Ticket>> Get()
        {
            return await Task.Run(() =>
            {
                return Tickets.Values.ToList();
            });
        }

        public async Task<List<Ticket>> Get(Func<List<Ticket>, List<Ticket>> filter)
        {
            return await Task.Run(() =>
            {
                List<Ticket> tickets = Tickets.Values.ToList();

                return filter != null ? filter(tickets) : tickets;
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
