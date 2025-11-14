namespace DomainModel
{
    public sealed class Tariff
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public bool RequiresSmartMeter { get; private set; }
        public decimal PricePerUnit { get; private set; }

        public Tariff(string id, string name, bool requiresSmartMeter, decimal pricePerUnit)
        {
            Guard.NotNullOrWhiteSpace(id, "id");
            Guard.NotNullOrWhiteSpace(name, "name");
            Guard.NonNegative(pricePerUnit, "pricePerUnit");

            Id = id;
            Name = name;
            RequiresSmartMeter = requiresSmartMeter;
            PricePerUnit = pricePerUnit;
        }
    }
}
