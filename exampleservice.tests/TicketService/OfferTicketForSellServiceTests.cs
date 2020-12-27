﻿using exampleservice.Framework.Abstract;
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
    public class OfferTicketForSelldServiceTests
    {
        [Test]
        public async Task OfferTicketForSellStepSucceed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(createTicket(ticketNumber, false, true));
            var instanceUnderTest = new TicketService.TicketService(busMock.Object, database);
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
            await database.Add(createTicket("ticketNrNotInDB", true, false));
            var instanceUnderTest = new TicketService.TicketService(busMock.Object, database);
            var createTicketCommand = new OfferTicketForSellCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(CouldNotOfferTicketForSellEvent));
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
