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
            var database = new TicketStorageRepository();

            var instanceUnderTest = new exampleservice.TicketService.TicketService(busMock.Object, database);

            string ticketNumber = "Ticket#1";

            var ticket = new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = ticketNumber,
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train"
            };

            var createTicketCommand = new CreateTicketCommand
            {
                Ticket = ticket
            };

            var resultedEvent = await instanceUnderTest.Handle(createTicketCommand);

            using (new AssertionScope())
            {
                resultedEvent.Should().BeOfType(typeof(TicketCreatedEvent));

                var ticketCreatedEvent = (TicketCreatedEvent)resultedEvent;
                ticketCreatedEvent.TicketNumber.Should().BeSameAs(ticketNumber);

                ticket.Id.Should().NotBe(null);
            }
        }

        [Test]
        public async Task Execute_TicketServiceCreateOk_TicketIdIsSet()
        {
            var database = new TicketStorageRepository();

            var ticket = new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#1",
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train"
            };

            var ticket2 = new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = "Ticket#2",
                FromLocationId = 456,
                ToLocationId = 654,
                MeansOfTransport = "Train"
            };

            await database.Add(ticket);
            await database.Add(ticket2);

            using (new AssertionScope())
            {
                ticket.Id.Should().NotBe(null);
                ticket2.Id.Should().NotBe(null);

                ticket.Id.Should().NotBe(ticket2.Id);
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

        [Test]
        public async Task Execute_TicketServiceCreateNotOk_TicketNumberNotUniqe()
        {
            var ticketNumber = "Ticket#1";

            var ticket = new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = ticketNumber,
                FromLocationId = 123,
                ToLocationId = 321,
                MeansOfTransport = "Train"
            };

            var ticket2 = new Ticket
            {
                CreateDate = new DateTime(),
                TicketNumber = ticketNumber,
                FromLocationId = 456,
                ToLocationId = 654,
                MeansOfTransport = "Train"
            };

            var database = new TicketStorageRepository();

            var instanceUnderTest = new exampleservice.TicketService.Steps.CreateTicketStep(database);

            var context = new TicketContext
            {
                Command = new CreateTicketCommand
                { Ticket = ticket }
            };

            await instanceUnderTest.Execute(context);

            context = new TicketContext
            {
                Command = new CreateTicketCommand
                { Ticket = ticket2 }
            };

            await instanceUnderTest.Execute(context);

            using (new AssertionScope())
            {
                context.TicketWasCreated.Should().BeFalse();
                context.WasCompensated.Should().BeTrue();
            }
        }
    }
}
