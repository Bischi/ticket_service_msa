// LoadTicketsStep.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exampleservice.TicketService.Models;
using exampleservice.TicketService.Repositories;
using simplescript.Abstract;

namespace exampleservice.TicketService.Steps
{
    public class LoadTicketsStep : ProcedureStepBase<GetTicketsContext>
    {
        private ITicketStorageRepository dataRepository;
        public LoadTicketsStep(ITicketStorageRepository dataRepository) => this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));

        protected override async Task<bool> StepSpecificExecute(GetTicketsContext contextType)
        {
            contextType.Tickets = contextType.Command.OnlySoldTickets
                ? await dataRepository.Get((tickets) =>
                {
                    return tickets.Where(x => !x.isAvailable).ToList();
                })
                : await dataRepository.Get();

            return true;
        }
    }
}
