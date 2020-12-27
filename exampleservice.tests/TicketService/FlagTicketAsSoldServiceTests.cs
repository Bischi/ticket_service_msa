using exampleservice.Framework.Abstract;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Models;
using exampleservice.TicketService.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace exampleservice.tests.SellTicketService
{
    [TestFixture]
    public class FlagTicketAsSoldServiceTests
    {
        [Test]
        public async Task FlagTicketAsSoldStepSucceed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(createTicket(ticketNumber, true, true));
            var instanceUnderTest = new TicketService.TicketService(busMock.Object, database);
            var createTicketCommand = new FlagTicketAsSoldCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);
      
            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(FlagedTicketAsSoldEvent));
            }
        }

        [Test]
        public async Task FlagTicketAsSoldStepFailed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(createTicket("ticketNrNotInDB", true, true));
            var instanceUnderTest = new TicketService.TicketService(busMock.Object, database);
            var createTicketCommand = new FlagTicketAsSoldCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(CouldNotFlagTicketAsSoldEvent));
            }
        }


        private Ticket createTicket(string ticketNumber, bool hasOffer, bool isAvailable)
        {
            var ticket = new Ticket();
            ticket.TicketNumber = ticketNumber;
            ticket.hasOffer = hasOffer;
            ticket.isAvailable = isAvailable;
            return ticket;
        }
    }
}
