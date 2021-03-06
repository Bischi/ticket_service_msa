﻿// ITicketStorageRepository.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using exampleservice.TicketService.Models;

namespace exampleservice.TicketService.Repositories
{
    public interface ITicketStorageRepository
    {
        public Task<List<Ticket>> Get();
        public Task<List<Ticket>> Get(Func<List<Ticket>, List<Ticket>> filter);
        public Task<bool> Add(Ticket ticket);
        public Task<Ticket> Get(string ticketNumber);
        public Task<int> Save(Ticket ticket);
    }
}
