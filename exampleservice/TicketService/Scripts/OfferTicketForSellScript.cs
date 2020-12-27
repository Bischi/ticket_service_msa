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
    class OfferTicketForSellScript
    {
        private Lazy<Procedure<OfferTicketForSellContext>> procedure;
        private IMessageBus bus;
        private ITicketStorageRepository dataBaseRepository;

        public OfferTicketForSellScript(IMessageBus bus, ITicketStorageRepository dataBaseRepository)
        {
            procedure = new Lazy<Procedure<OfferTicketForSellContext>>(() => this.GetProcedure());
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.dataBaseRepository = dataBaseRepository ?? throw new ArgumentNullException(nameof(dataBaseRepository));
        }

        public async Task<EventBase> Handle(OfferTicketForSellCommand command)
        {
            this.VerifyInputArguments(command);
            var context = new OfferTicketForSellContext() { Command = command };
            await procedure.Value.Execute(context);

            if (context.WasCompensated)
            {
                return new CouldNotOfferTicketForSellEvent();
            }
            else
            {
                return new OfferedTicketForSellEvent { Message = context.Command.TicketNumber + " was included in an offer." };
            }
        }

        private void VerifyInputArguments(OfferTicketForSellCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
        }

        private Procedure<OfferTicketForSellContext> GetProcedure()
        {
            var offerTicketForSellStep = new OfferTicketForSellStep(dataBaseRepository);

            return ProcedureDescription<OfferTicketForSellContext>.
                Start().
                Then(offerTicketForSellStep).
                Finish();
        }
    }
}
