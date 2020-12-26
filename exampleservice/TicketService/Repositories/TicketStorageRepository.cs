// TicketStoreRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System.Collections.Concurrent;
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
    }
}
