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

namespace exampleservice.TicketService
{
    public class TicketService
    {
        private Lazy<Procedure<TicketContext>> procedure;
        private IMessageBus bus;
        private ITicketStorageRepository dataBaseRepository;

        public TicketService(IMessageBus bus, ITicketStorageRepository dataBaseRepository)
        {
            procedure = new Lazy<Procedure<TicketContext>>(() => this.GetProcedure());
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.dataBaseRepository = dataBaseRepository ?? throw new ArgumentNullException(nameof(dataBaseRepository));
        }

        public async Task<EventBase> Handle(CreateTicketCommand command)
        {
            this.VerifyIputArguments(command);

            var context = new TicketContext() { Command = command };

            await procedure.Value.Execute(context);

            if (context.WasCompensated)
            {
                return new CouldNotCreateTicketEvent();
            }

            return null;
        }

        private void VerifyIputArguments(CreateTicketCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
        }

        private Procedure<TicketContext> GetProcedure()
        {
            //var withdrawFromBuyerStep = new WithdrawFromBuyerAccount(this.bus);
            //var depositToSellerStep = new DepositToSellerAccount(this.bus);
            //var sellTicketStep = new SellTicket(this.bus);
            //var saveSoldTicketStep = new SaveSoldTicketWithOptimisticLock(this.dataBaseRepository);
            //return ProcedureDescription<SellTicketContext>.
            //   Start().
            //   Then(withdrawFromBuyerStep).
            //   Then(depositToSellerStep).
            //   Then(sellTicketStep).
            //   Then(saveSoldTicketStep).
            //   Finish();

            var createTicketStepTEST = new CreateTicketStepTEST(this.bus);

            return ProcedureDescription<TicketContext>.
               Start().
               Then(createTicketStepTEST).
               Finish();
        }
    }
}