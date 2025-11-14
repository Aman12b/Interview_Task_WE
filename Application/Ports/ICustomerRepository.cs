using DomainModel;

namespace Application.Ports
{
    public interface ICustomerRepository
    {
        Customer GetById(string id);
    }
}
