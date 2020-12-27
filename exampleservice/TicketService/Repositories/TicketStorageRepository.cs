// TicketStoreRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Repositories
{
    public class TicketStorageRepository : ITicketStorageRepository
    {
        // Id, Ticket
        private ConcurrentDictionary<int, Ticket> Tickets;

        public TicketStorageRepository()
        {
            Tickets = new ConcurrentDictionary<int, Ticket>();
        }

        public async Task<int> Add(Ticket ticket)
        {
            return await Task.Run(() =>
            {
                Tickets.TryAdd(ticket.Id, ticket);
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
    }
}
