using exampleservice.TicketService.Repositories;
using simplescript.Abstract;
using System.Threading.Tasks;

namespace exampleservice.TicketService.Steps
{
    public class OfferTicketForSellStep : ProcedureStepBase<OfferTicketForSellContext>
    {
        private ITicketStorageRepository dataBaseRepository;

        public OfferTicketForSellStep(ITicketStorageRepository rep)
        {
            dataBaseRepository = rep;
        }

        protected override async Task<bool> StepSpecificExecute(OfferTicketForSellContext contextType)
        {
            var ticket = await dataBaseRepository.Get(contextType.Command.TicketNumber);
            if (ticket == null)
            {
                contextType.WasCompensated = true;
                return true;
            }

            if (ticket.HasOffer || !ticket.IsAvailable)
            {
                contextType.WasCompensated = true;
                return true;
            }

            ticket.HasOffer = true;

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
