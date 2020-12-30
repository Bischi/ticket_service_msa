using exampleservice.AccoutingService.Contract;
using exampleservice.Framework.Abstract;
using exampleservice.TicketService;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Models;
using exampleservice.TicketService.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace exampleservice.tests.TicketService
{
    [TestFixture]
    public class TicketServiceTests
    {
        [Test]
        public async Task CreateTicketStepSucceed()
        {
            var busMock = new Moq.Mock<IMessageBus>();
            busMock.Setup(s => s.RequestAndReply<WithdrawFromCustomerCommand>(It.IsAny<WithdrawFromCustomerCommand>())).
                ReturnsAsync(new WithdrawnFromCustomerEvent());

            busMock.Setup(s => s.RequestAndReply<DepositToCustomerCommand>(It.IsAny<DepositToCustomerCommand>())).
                ReturnsAsync(new DepositedToCustomerEvent());

            busMock.Setup(s => s.RequestAndReply<FlagTicketAsSoldCommand>(It.IsAny<FlagTicketAsSoldCommand>())).
                ReturnsAsync(new FlagedTicketAsSoldEvent());

            var database = new TicketStorageRepository();

            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);
            
            string ticketNumber = "Ticket#1";

            var createTicketCommand = new CreateTicketCommand
            {
                Ticket = new Ticket
                {
                    Id = 1,
                    CreateDate = new DateTime(),
                    TicketNumber = ticketNumber,
                    FromLocationId = 123,
                    ToLocationId = 321,
                    MeansOfTransport = "Train"
                }
            };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(TicketCreatedEvent));

                var ticketCreatedEvent = (TicketCreatedEvent)resultedEvent;
                ticketCreatedEvent.TicketNumber.Should().BeSameAs(ticketNumber);
            }
        }

        [Test]
        public async Task Execute_TicketServiceCreateOk_ReturnOkEvent()
        {
            var number = "Ticket#2";

            var database = new TicketStorageRepository();

            var instanceUnderTest = new exampleservice.TicketService.Steps.CreateTicketStep(database);
            var context = new TicketContext { Command = new CreateTicketCommand { Ticket = new Ticket { TicketNumber = number } } };
            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.TicketWasCreated.Should().BeTrue();
                context.WasCompensated.Should().BeFalse();
            }
        }
    }
}
