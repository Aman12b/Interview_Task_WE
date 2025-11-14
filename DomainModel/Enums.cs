using System;

namespace DomainModel
{
    public enum SlaLevel
    {
        Standard = 0,
        Premium = 1
    }

    public enum MeterType
    {
        Classic = 0,
        Smart = 1
    }

    public enum FollowUpType
    {
        ScheduleMeterUpgrade = 0
    }

    public enum DecisionKind
    {
        Approved = 0,
        Rejected = 1,
        Skipped = 2
    }
}
