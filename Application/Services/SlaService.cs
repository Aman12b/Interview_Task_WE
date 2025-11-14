using System;
using DomainModel;
using Application.Ports;

namespace Application.Services
{
    public class SlaService
    {
        private readonly ITimeProvider _time;

        public SlaService(ITimeProvider timeProvider)
        {
            _time = timeProvider;
        }

        /// <summary>
        /// Berechnet die Deadline basierend auf SLA + evtl. Smart-Meter-Upgrade.
        /// </summary>
        public DateTimeOffset CalculateDue(Customer customer, Request request, bool hasSmartMeterUpgrade)
        {
            int baseHours = customer.Sla == SlaLevel.Premium ? 24 : 48;
            if (hasSmartMeterUpgrade)
            {
                baseHours += 12;
            }

            // Vereinfachung: RequestedAt ist bereits in "korrekter" lokaler Zeit
            return request.RequestedAt.AddHours(baseHours);
        }
    }
}
