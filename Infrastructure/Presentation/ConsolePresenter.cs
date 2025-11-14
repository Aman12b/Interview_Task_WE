using System;
using System.Collections.Generic;
using Application.Ports;
using DomainModel;

namespace Infrastructure.Presentation
{
    public class ConsolePresenter : IOutput
    {
        public void WriteResult(Request request, Decision decision, IEnumerable<FollowUpAction> followUps)
        {
            Console.WriteLine(
                "[{0}] Request {1} for Customer {2} -> {3} (Reason: {4}, SLA Due: {5})",
                request.RequestedAt,
                request.RequestId,
                request.CustomerId,
                decision.Kind,
                string.IsNullOrEmpty(decision.Reason) ? "-" : decision.Reason,
                decision.SlaDueAt.HasValue ? decision.SlaDueAt.Value.ToString("u") : "-"
            );

            if (followUps != null)
            {
                foreach (FollowUpAction f in followUps)
                {
                    Console.WriteLine("   FollowUp: {0} for Request {1}, Due: {2}",
                        f.Type,
                        f.RequestId,
                        f.DueAt.HasValue ? f.DueAt.Value.ToString("u") : "-"
                    );
                }
            }
        }

        public void WriteSummary(int total, int approved, int rejected, int skipped, int skippedAlreadyProcessed)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Summary:");
            Console.WriteLine("  Total requests:          {0}", total);
            Console.WriteLine("  Approved:                {0}", approved);
            Console.WriteLine("  Rejected:                {0}", rejected);
            Console.WriteLine("  Skipped (other):         {0}", skipped);
            Console.WriteLine("  Skipped (already done):  {0}", skippedAlreadyProcessed);
        }
    }
}
