// CreateTicketStep.cs
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
    public class CreateTicketStep : ProcedureStepBase<TicketContext>
    {
        private ITicketStorageRepository dataRepository;
        public CreateTicketStep(ITicketStorageRepository dataRepository) => this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        protected override async Task<bool> StepSpecificExecute(TicketContext contextType)
        {
            var reply = await dataRepository.Add(contextType.Command.Ticket);

            if (reply is false)
            {
                await CompensatePredecssorOnly(contextType);
                contextType.WasCompensated = true;
                return true;
            }
            else
            {
                contextType.TicketWasCreated = true;
                return false;
            }
        }
    }
}
