using exampleservice.Framework.Abstract;
using exampleservice.TicketService.Contracts;
using exampleservice.TicketService.Models;
using exampleservice.TicketService.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace exampleservice.tests.TicketService
{
    [TestFixture]
    public class GetTicketsTest
    {
        private exampleservice.TicketService.TicketService instanceUnderTest;

        [OneTimeSetUp]
        public async Task Init()
        {
            var busMock = new Moq.Mock<IMessageBus>();
            var database = new TicketStorageRepository();

            instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);

            await database.Add(new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#1",
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train",
                HasOffer = false,
                IsAvailable = true
            });

            await database.Add(new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#2",
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train",
                HasOffer = false,
                IsAvailable = false
            });

        }

        [Test]
        public async Task GetTicketsStepSucceed()
        {
            var getTicketsCommand = new GetTicketsCommand();

            var getTicketsResultEvent = await instanceUnderTest.Handle(getTicketsCommand);

            using (new AssertionScope())
            {
                getTicketsResultEvent.Should().BeOfType(typeof(TicketsLoadedEvent));

                var ticketsLoadedEvent = (TicketsLoadedEvent)getTicketsResultEvent;
                ticketsLoadedEvent.Tickets.Should().HaveCountGreaterThan(0);
                ticketsLoadedEvent.Tickets.Should().HaveCount(2);

                var ticket1Test = ticketsLoadedEvent.Tickets.Where(x => x.TicketNumber.Equals("Ticket#1")).Single();
                var ticket2Test = ticketsLoadedEvent.Tickets.Where(x => x.TicketNumber.Equals("Ticket#2")).Single();

                ticket1Test.Should().NotBeNull();
                ticket2Test.Should().NotBeNull();
            }
        }

        [Test]
        public async Task GetSoldTicketsSucceed()
        {
            var getTicketsCommand = new GetTicketsCommand { OnlySoldTickets = true };
            var getTicketsResultEvent = await instanceUnderTest.Handle(getTicketsCommand);

            using (new AssertionScope())
            {
                getTicketsResultEvent.Should().BeOfType(typeof(TicketsLoadedEvent));

                var ticketsLoadedEvent = (TicketsLoadedEvent)getTicketsResultEvent;
                ticketsLoadedEvent.Tickets.Should().HaveCount(1);

                var soldTicket = ticketsLoadedEvent.Tickets.Single(x => x.TicketNumber.Equals("Ticket#2"));

                soldTicket.Should().NotBeNull();
                soldTicket.IsAvailable.Should().BeFalse();
            }
        }

        [Test]
        public async Task GetAvailableTicketsSucceed()
        {
            var getTicketsCommand = new GetTicketsCommand { OnlyAvailableTickets = true };
            var getTicketsResultEvent = await instanceUnderTest.Handle(getTicketsCommand);

            using (new AssertionScope())
            {
                getTicketsResultEvent.Should().BeOfType(typeof(TicketsLoadedEvent));

                var ticketsLoadedEvent = (TicketsLoadedEvent)getTicketsResultEvent;
                ticketsLoadedEvent.Tickets.Should().HaveCount(1);

                var soldTicket = ticketsLoadedEvent.Tickets.Single(x => x.TicketNumber.Equals("Ticket#1"));

                soldTicket.Should().NotBeNull();
                soldTicket.IsAvailable.Should().BeTrue();
            }
        }
    }
}
