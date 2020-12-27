using exampleservice.AccoutingService.Contract;
using exampleservice.Framework.Abstract;
using exampleservice.SellTicketService.Contract;
using exampleservice.SellTicketService.Controller;
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
    public class GetTicketsTest
    {
        private exampleservice.TicketService.TicketService instanceUnderTest;

        [OneTimeSetUp]
        public async Task Init()
        {
            var busMock = new Moq.Mock<IMessageBus>();
            var database = new TicketStorageRepository();

            instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);

            // TODO -> Id of Ticket should get set in Repository like with EF!!

            await database.Add(new Ticket
            {
                Id = 1,
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#1",
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train"
            });

            await database.Add(new Ticket
            {
                Id = 2,
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#2",
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train"
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
            }
        }
    }
}
