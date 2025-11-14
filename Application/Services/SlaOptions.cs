namespace Application.Services
{
    /// <summary>
    /// Konfiguration der SLA-Zeiten in Stunden.
    /// </summary>
    public sealed class SlaOptions
    {
        public int StandardHours { get; set; }
        public int PremiumHours { get; set; }
        public int SmartMeterUpgradeExtraHours { get; set; }
    }
}
