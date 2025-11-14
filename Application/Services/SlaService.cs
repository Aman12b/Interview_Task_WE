using System;
using Application.Ports;
using DomainModel;

namespace Application.Services
{
    public class SlaService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly SlaOptions _options;

        public SlaService(ITimeProvider timeProvider, SlaOptions options)
        {
            if (timeProvider == null) throw new ArgumentNullException("timeProvider");
            if (options == null) throw new ArgumentNullException("options");

            _timeProvider = timeProvider;
            _options = options;
        }

        public DateTimeOffset CalculateDue(Customer customer, Request request, bool hasSmartMeterUpgrade)
        {
            int baseHours;

            if (customer.Sla == SlaLevel.Premium)
                baseHours = _options.PremiumHours;
            else
                baseHours = _options.StandardHours;

            if (hasSmartMeterUpgrade)
                baseHours += _options.SmartMeterUpgradeExtraHours;

            // Referenz ist weiterhin RequestedAt
            return request.RequestedAt.AddHours(baseHours);
        }
    }
}
