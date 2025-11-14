using System;

namespace DomainModel
{
    public sealed class FollowUpAction
    {
        public string Id { get; private set; }
        public FollowUpType Type { get; private set; }
        public string RequestId { get; private set; }
        public DateTimeOffset? DueAt { get; private set; }

        private FollowUpAction(string id, FollowUpType type, string requestId, DateTimeOffset? dueAt)
        {
            Guard.NotNullOrWhiteSpace(id, "id");
            Guard.NotNullOrWhiteSpace(requestId, "requestId");
            Id = id;
            Type = type;
            RequestId = requestId;
            DueAt = dueAt;
        }

        public static string NewId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static FollowUpAction Create(string id, FollowUpType type, string requestId, DateTimeOffset? dueAt)
        {
            return new FollowUpAction(id, type, requestId, dueAt);
        }

        public void SetDue(DateTimeOffset due)
        {
            DueAt = due;
        }
    }
}
