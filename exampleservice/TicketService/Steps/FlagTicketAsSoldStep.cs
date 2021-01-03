using exampleservice.TicketService.Repositories;
using simplescript.Abstract;
using System.Threading.Tasks;

namespace exampleservice.TicketService.Steps
{
    public class FlagTicketAsSoldStep : ProcedureStepBase<FlagTicketAsSoldContext>
    {
        private ITicketStorageRepository dataBaseRepository;

        public FlagTicketAsSoldStep(ITicketStorageRepository rep)
        {
            dataBaseRepository = rep;
        }

        protected override async Task<bool> StepSpecificExecute(FlagTicketAsSoldContext contextType)
        {
            var ticket = await dataBaseRepository.Get(contextType.Command.TicketNumber);
            if (ticket == null)
            {
                contextType.WasCompensated = true;
                return true;
            }

            if (!ticket.HasOffer || !ticket.IsAvailable)
            {
                contextType.WasCompensated = true;
                return true;
            }

            ticket.IsAvailable = false;
            ticket.HasOffer = false;

            int affectedRows = await dataBaseRepository.Save(ticket);
            if (affectedRows == 0)
            {
                contextType.WasCompensated = true;
                return true;
            }

            return false;
        }
    }
}
