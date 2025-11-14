using Application.Ports;

namespace Application.Services
{
    public class IdempotencyService
    {
        private readonly IProcessedLedgerStore _ledger;

        public IdempotencyService(IProcessedLedgerStore ledger)
        {
            _ledger = ledger;
        }

        public bool ShouldProcess(string requestId)
        {
            return !_ledger.IsProcessed(requestId);
        }

        public void MarkProcessed(string requestId)
        {
            _ledger.MarkProcessed(requestId);
        }
    }
}
