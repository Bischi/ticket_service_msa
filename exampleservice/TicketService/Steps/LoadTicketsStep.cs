// LoadTicketsStep.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Threading.Tasks;
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
            var data = await dataRepository.Get();
            contextType.Tickets = data;
            return true;
        }
    }
}
