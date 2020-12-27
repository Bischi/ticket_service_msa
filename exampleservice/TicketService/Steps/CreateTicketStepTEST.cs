// CreateTicketStepTEST.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Threading.Tasks;
using exampleservice.Framework.Abstract;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Repositories;
using simplescript.Abstract;

namespace exampleservice.TicketService.Steps
{
    public class CreateTicketStepTEST : ProcedureStepBase<TicketContext>
    {
        private ITicketStorageRepository dataRepository;
        public CreateTicketStepTEST(ITicketStorageRepository dataRepository) => this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));

        protected override async Task<bool> StepSpecificExecute(TicketContext contextType)
        {
            var reply = await dataRepository.Add(contextType.Command.Ticket);

            return reply == 1;
        }
    }
}
