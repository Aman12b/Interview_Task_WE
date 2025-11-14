namespace Application.Ports
{
    public interface IProcessedLedgerStore
    {
        bool IsProcessed(string requestId);
        void MarkProcessed(string requestId);
    }
}
