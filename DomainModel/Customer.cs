namespace DomainModel
{
    public sealed class Customer
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public bool HasUnpaidInvoices { get; private set; }
        public SlaLevel Sla { get; private set; }
        public MeterType Meter { get; private set; }

        public Customer(string id, string name, bool hasUnpaidInvoices, SlaLevel sla, MeterType meter)
        {
            Guard.NotNullOrWhiteSpace(id, "id");
            Guard.NotNullOrWhiteSpace(name, "name");
            Id = id;
            Name = name;
            HasUnpaidInvoices = hasUnpaidInvoices;
            Sla = sla;
            Meter = meter;
        }

        public Customer WithMeter(MeterType meter)
        {
            return new Customer(Id, Name, HasUnpaidInvoices, Sla, meter);
        }
    }
}
