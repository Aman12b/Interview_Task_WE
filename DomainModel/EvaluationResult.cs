using System.Collections.Generic;

namespace DomainModel
{
    public sealed class EvaluationResult
    {
        public Decision Decision { get; private set; }
        public IReadOnlyList<FollowUpAction> FollowUps { get; private set; }

        private EvaluationResult(Decision decision, IReadOnlyList<FollowUpAction> followUps)
        {
            Decision = decision;
            FollowUps = followUps ?? new FollowUpAction[0];
        }

        public static EvaluationResult Approved()
        {
            return new EvaluationResult(Decision.Approved(null), new FollowUpAction[0]);
        }

        public static EvaluationResult ApprovedWithFollowUp(FollowUpAction action)
        {
            return new EvaluationResult(Decision.Approved(null), new FollowUpAction[] { action });
        }

        public static EvaluationResult Rejected(string reason)
        {
            return new EvaluationResult(Decision.Rejected(reason), new FollowUpAction[0]);
        }
    }
}
