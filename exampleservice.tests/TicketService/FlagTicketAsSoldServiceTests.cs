﻿using exampleservice.Framework.Abstract;
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
    public class FlagTicketAsSoldServiceTests
    {
        [Test]
        public async Task FlagTicketAsSoldStepSucceed()
        {
            var busMock = new Mock<IMessageBus>();
            string ticketNumber = "Ticket#1";
            var database = new TicketStorageRepository();
            await database.Add(CreateTicket(ticketNumber, true, true));
            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);
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
            await database.Add(CreateTicket("ticketNrNotInDB", true, true));
            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);
            var createTicketCommand = new FlagTicketAsSoldCommand { TicketNumber = ticketNumber };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(CouldNotFlagTicketAsSoldEvent));
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
