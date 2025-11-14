using System.Collections.Generic;
using DomainModel;

namespace Application.Ports
{
    public interface IFollowUpActionStore
    {
        void SaveMany(IEnumerable<FollowUpAction> actions);
    }
}
