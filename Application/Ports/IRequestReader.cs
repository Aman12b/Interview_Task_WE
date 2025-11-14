using System.Collections.Generic;
using DomainModel;

namespace Application.Ports
{
    public interface IRequestReader
    {
        IEnumerable<Request> ReadNew();
    }
}
