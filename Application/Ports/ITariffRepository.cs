using DomainModel;

namespace Application.Ports
{
    public interface ITariffRepository
    {
        Tariff GetById(string id);
    }
}
