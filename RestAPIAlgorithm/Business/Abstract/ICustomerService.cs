using RestAPIAlgorithm.Model;

namespace RestAPIAlgorithm.Business.Abstract
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> AddCustomers (List<CustomerDto> customers);
        List<CustomerDto> GetCustomers();
        short? GetLastId();
    }
}
