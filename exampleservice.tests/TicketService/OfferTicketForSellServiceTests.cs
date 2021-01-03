using exampleservice.Framework.Abstract;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Models;
using exampleservice.TicketService.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace exampleservice.tests.TicketService
{
    [TestFixture]
    public class OfferTicketForSelldServiceTests
    {
        [Test]
        public async Task OfferTicketForSellStepSucceed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(CreateTicket(ticketNumber, false, true));
            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);
            var offerTicketForSellCommand = new OfferTicketForSellCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(offerTicketForSellCommand);
      
            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(OfferedTicketForSellEvent));
            }
        }

        [Test]
        public async Task FlagTicketAsSoldStepFailed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(CreateTicket("ticketNrNotInDB", true, false));
            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);
            var createTicketCommand = new OfferTicketForSellCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(CouldNotOfferTicketForSellEvent));
            }
        }

        private Ticket CreateTicket(string ticketNumber, bool hasOffer, bool isAvailable)
        {
            var ticket = new Ticket
            {
                TicketNumber = ticketNumber,
                HasOffer = hasOffer,
                IsAvailable = isAvailable
            };
            return ticket;
        }
    }
}
