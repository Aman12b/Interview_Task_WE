using System;

namespace DomainModel
{
    public sealed class Request
    {
        public string RequestId { get; private set; }
        public string CustomerId { get; private set; }
        public string TargetTariffId { get; private set; }
        public DateTimeOffset RequestedAt { get; private set; }

        public Request(string requestId, string customerId, string targetTariffId, DateTimeOffset requestedAt)
        {
            Guard.NotNullOrWhiteSpace(requestId, "requestId");
            Guard.NotNullOrWhiteSpace(customerId, "customerId");
            Guard.NotNullOrWhiteSpace(targetTariffId, "targetTariffId");

            RequestId = requestId;
            CustomerId = customerId;
            TargetTariffId = targetTariffId;
            RequestedAt = requestedAt;
        }

        public PreliminaryDecision Decide(Customer customer, Tariff tariff)
        {
            if (customer == null)
                return PreliminaryDecision.Rejected("Customer not found");

            if (tariff == null)
                return PreliminaryDecision.Rejected("Tariff not found");

            if (customer.HasUnpaidInvoices)
                return PreliminaryDecision.Rejected("Customer has unpaid invoices");

            if (tariff.RequiresSmartMeter && customer.Meter == MeterType.Classic)
            {
                FollowUpAction followUp = FollowUpAction.Create(
                    FollowUpAction.NewId(),
                    FollowUpType.ScheduleMeterUpgrade,
                    RequestId,
                    null
                );

                return PreliminaryDecision.ApprovedWithFollowUp(followUp);
            }

            return PreliminaryDecision.Approved();
        }
    }
}
