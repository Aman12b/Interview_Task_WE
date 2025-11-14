using System.Collections.Generic;
using DomainModel;

namespace Application.Ports
{
    public interface IOutput
    {
        void WriteResult(Request request, Decision decision, IEnumerable<FollowUpAction> followUps);
        void WriteSummary(int total, int approved, int rejected, int skipped, int skippedAlreadyProcessed);
    }
}
