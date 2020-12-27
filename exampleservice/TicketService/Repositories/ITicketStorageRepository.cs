// ITicketStorageRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Threading.Tasks;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Repositories
{
    public interface ITicketStorageRepository
    {
        public Task<int> Add(Ticket ticket);

        public Task<Ticket> Get(string ticketNumber);

        public Task<int> Save(Ticket ticket);
    }
}
