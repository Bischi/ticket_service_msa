using exampleservice.Framework.Abstract;
using exampleservice.Framework.BaseFramework;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Repositories;
using exampleservice.TicketService.Steps;
using simplescript;
using simplescript.DSL;
using System;
using System.Threading.Tasks;

namespace exampleservice.TicketService.Scripts
{
    class FlagTicketAsSoldScript
    {
        private Lazy<Procedure<FlagTicketAsSoldContext>> procedure;
        private IMessageBus bus;
        private ITicketStorageRepository dataBaseRepository;

        public FlagTicketAsSoldScript(IMessageBus bus, ITicketStorageRepository dataBaseRepository)
        {
            procedure = new Lazy<Procedure<FlagTicketAsSoldContext>>(() => this.GetProcedure());
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.dataBaseRepository = dataBaseRepository ?? throw new ArgumentNullException(nameof(dataBaseRepository));
        }

        public async Task<EventBase> Handle(FlagTicketAsSoldCommand command)
        {
            this.VerifyInputArguments(command);
            var context = new FlagTicketAsSoldContext() { Command = command };
            await procedure.Value.Execute(context);

            if (context.WasCompensated)
            {
                return new CouldNotFlagTicketAsSoldEvent();
            }
            else
            {
                return new FlagedTicketAsSoldEvent { Message = context.Command.TicketNumber + " was flagged as sold" };
            }
        }

        private void VerifyInputArguments(FlagTicketAsSoldCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
        }

        private Procedure<FlagTicketAsSoldContext> GetProcedure()
        {
            var flagTicketAsSoldStep = new FlagTicketAsSoldStep(dataBaseRepository);

            return ProcedureDescription<FlagTicketAsSoldContext>.
                Start().
                Then(flagTicketAsSoldStep).
                Finish();
        }
    }
}
