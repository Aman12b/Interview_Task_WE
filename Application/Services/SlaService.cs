using System;
using Application.Ports;
using DomainModel;

namespace Application.Services
{
    public class SlaService
    {
        private readonly SlaOptions _options;

        public SlaService(SlaOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

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

            var utc = request.RequestedAt.UtcDateTime.AddHours(baseHours);
            return new DateTimeOffset(utc, TimeSpan.Zero)
                .ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Europe/Vienna").GetUtcOffset(utc));
        }
    }
}
