// CreateTicketStepTEST.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Threading.Tasks;
using exampleservice.Framework.Abstract;
using exampleservice.TicketService.Contracts;
using simplescript.Abstract;

namespace exampleservice.TicketService.Steps
{
    public class CreateTicketStepTEST : ProcedureStepBase<TicketContext>
    {
        private IMessageBus bus;

        public CreateTicketStepTEST(IMessageBus bus) => this.bus = bus ?? throw new ArgumentNullException(nameof(bus));

        protected override async Task<bool> StepSpecificExecute(TicketContext contextType)
        {
            var reply = await this.bus.RequestAndReply(new CreateTicketCommand());

            return true;
        }
    }
}
