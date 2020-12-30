// TicketService.cs
// Author: Martin Bischof <bischi@live.at>
// Created: 12/26/2020
//
//
using System;
using System.Threading.Tasks;
using exampleservice.Framework.Abstract;
using exampleservice.Framework.BaseFramework;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Repositories;
using exampleservice.TicketService.Steps;
using simplescript;
using simplescript.DSL;

namespace exampleservice.TicketService.Scripts
{
    public class GetTicketsScript
    {
        private Lazy<Procedure<GetTicketsContext>> procedure;
        private IMessageBus bus;
        private ITicketStorageRepository dataBaseRepository;

        public GetTicketsScript(IMessageBus bus, ITicketStorageRepository dataBaseRepository)
        {
            procedure = new Lazy<Procedure<GetTicketsContext>>(() => this.GetProcedure());
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.dataBaseRepository = dataBaseRepository ?? throw new ArgumentNullException(nameof(dataBaseRepository));
        }

        public async Task<EventBase> Handle(GetTicketsCommand command)
        {
            this.VerifyIputArguments(command);

            var context = new GetTicketsContext() { Command = command };

            await procedure.Value.Execute(context);

            return new TicketsLoadedEvent { Tickets = context.Tickets };
        }

        private void VerifyIputArguments(GetTicketsCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
        }

        private Procedure<GetTicketsContext> GetProcedure()
        {
            var loadTicketsStep = new LoadTicketsStep(dataBaseRepository);

            return ProcedureDescription<GetTicketsContext>.
               Start().
               Then(loadTicketsStep).
               Finish();
        }
    }
}