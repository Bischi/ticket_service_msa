using exampleservice.TicketService;
using exampleservice.TicketService.Steps;
using exampleservice.TicketService.Contracts;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System.Threading.Tasks;

namespace exampleservice.tests.TicketService.Steps
{
    [TestFixture]
    public class OfferTicketForSellStepTests
    {
        [Test]
        public async Task Execute_TicketAlreadySold_ReturnCompensatedContext()
        {
            string ticketNumber = "MyTicketNumber";
            var database = new exampleservice.TicketService.Repositories.TicketStorageRepository();
            await database.Add(createTicket(ticketNumber, false, false));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeTrue();
            }
        }

        [Test]
        public async Task Execute_TicketAlreadyOffered_ReturnCompensatedContext()
        {
            string ticketNumber = "MyTicketNumber";
            var database = new exampleservice.TicketService.Repositories.TicketStorageRepository();
            await database.Add(createTicket(ticketNumber, true, true));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeTrue();
            }
        }

        [Test]
        public async Task Execute_ValidTicket_ReturnOkContext()
        {
            string ticketNumber = "MyTicketNumber";
            var database = new exampleservice.TicketService.Repositories.TicketStorageRepository();
            await database.Add(createTicket(ticketNumber, false, true));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeFalse();
            }
        }

        [Test]
        public async Task Execute_TicketNotInDb_RReturnCompensatedContext()
        {
            string ticketNumber = "MyTicketNumber";
            var database = new exampleservice.TicketService.Repositories.TicketStorageRepository();
            await database.Add(createTicket("notValidTicketNR", true, true));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeTrue();
            }
        }

        private exampleservice.TicketService.Models.Ticket createTicket(string ticketNumber, bool hasOffer, bool isAvailable)
        {
            var ticket = new exampleservice.TicketService.Models.Ticket();
            ticket.TicketNumber = ticketNumber;
            ticket.HasOffer = hasOffer;
            ticket.IsAvailable = isAvailable;
            return ticket;
        }
    }
}
