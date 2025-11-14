using System;
using Application.Ports;

namespace Infrastructure.Time
{
    public class SystemTimeProviderVienna : ITimeProvider
    {
        public DateTimeOffset Now()
        {
            // einfach lokale Zeit
            return DateTimeOffset.Now;
        }
    }
}
