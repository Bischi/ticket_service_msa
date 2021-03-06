﻿using exampleservice.TicketService;
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
            await database.Add(CreateTicket(ticketNumber, false, false));
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
            await database.Add(CreateTicket(ticketNumber, true, true));
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
            await database.Add(CreateTicket(ticketNumber, false, true));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeFalse();
            }
        }

        [Test]
        public async Task Execute_TicketNotInDb_ReturnCompensatedContext()
        {
            string ticketNumber = "MyTicketNumber";
            var database = new exampleservice.TicketService.Repositories.TicketStorageRepository();
            await database.Add(CreateTicket("notValidTicketNR", true, true));
            var instanceUnderTest = new OfferTicketForSellStep(database);
            var context = new OfferTicketForSellContext { Command = new OfferTicketForSellCommand { TicketNumber = ticketNumber } };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.WasCompensated.Should().BeTrue();
            }
        }

        private exampleservice.TicketService.Models.Ticket CreateTicket(string ticketNumber, bool hasOffer, bool isAvailable)
        {
            var ticket = new exampleservice.TicketService.Models.Ticket
            {
                TicketNumber = ticketNumber,
                HasOffer = hasOffer,
                IsAvailable = isAvailable
            };
            return ticket;
        }
    }
}
