using System;

namespace DomainModel
{
    public sealed class Decision
    {
        private DateTimeOffset due;

        public DecisionKind Kind { get; private set; }
        public string Reason { get; private set; }
        public DateTimeOffset? SlaDueAt { get; private set; }

        private Decision(DecisionKind kind, string reason, DateTimeOffset? slaDueAt)
        {
            Kind = kind;
            Reason = reason ?? string.Empty;
            SlaDueAt = slaDueAt;
        }

        public Decision WithDue(DateTimeOffset due)
        {
            return new Decision(this.Kind, this.Reason, due);
        }

        public static Decision Approved(DateTimeOffset? due)
        {
            return new Decision(DecisionKind.Approved, string.Empty, due);
        }

        public static Decision Rejected(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason)) reason = "Rejected";
            return new Decision(DecisionKind.Rejected, reason, null);
        }

        public static Decision Skipped(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason)) reason = "Skipped";
            return new Decision(DecisionKind.Skipped, reason, null);
        }

        public bool IsApproved
        {
            get { return Kind == DecisionKind.Approved; }
        }
    }
}
