using System;

namespace Application.Ports
{
    public interface ITimeProvider
    {
        DateTimeOffset Now();
    }
}
