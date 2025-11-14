using System.Collections.Generic;

namespace DomainModel
{
    public sealed class PreliminaryDecision
    {
        public Decision Decision { get; private set; }
        public IReadOnlyList<FollowUpAction> FollowUps { get; private set; }

        private PreliminaryDecision(Decision decision, IReadOnlyList<FollowUpAction> followUps)
        {
            Decision = decision;
            FollowUps = followUps ?? new FollowUpAction[0];
        }

        public static PreliminaryDecision Approved()
        {
            return new PreliminaryDecision(Decision.Approved(null), new FollowUpAction[0]);
        }

        public static PreliminaryDecision ApprovedWithFollowUp(FollowUpAction action)
        {
            return new PreliminaryDecision(Decision.Approved(null), new FollowUpAction[] { action });
        }

        public static PreliminaryDecision Rejected(string reason)
        {
            return new PreliminaryDecision(Decision.Rejected(reason), new FollowUpAction[0]);
        }
    }
}
