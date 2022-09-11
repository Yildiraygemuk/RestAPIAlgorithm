using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestAPIAlgorithm.Business.Abstract;
using RestAPIAlgorithm.Helper.Sorting;
using RestAPIAlgorithm.Model;

namespace RestAPIAlgorithm.Business.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly string _customers;
        private readonly IDistributedCache _distributedCache;
        private List<CustomerDto> _customerList;
        public CustomerService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _customers = _distributedCache.GetString("customerList");
            _customerList = new List<CustomerDto>();
        }
        public async Task<List<CustomerDto>> AddCustomers(List<CustomerDto> customers)
        {
            if (!string.IsNullOrEmpty(_customers))
            {
                _customerList = JsonConvert.DeserializeObject<List<CustomerDto>>(_customers);
            }

            foreach (var item in customers)
            {
                //If there is no data from the same Id inside, it adds
                if (!_customerList.Any(x => x.Id == item.Id) || !_customerList.Any())
                    _customerList.Add(item);
            }
            _customerList = SortingHandler.NameSorter(customerDto: _customerList);
            //If there is data in the customerList, it serializes and adds to the cache. Otherwise, it returns the data returned from the cache.
            await _distributedCache.SetStringAsync("customerList", JsonConvert.SerializeObject(_customerList));
            return customers;
        }
        public List<CustomerDto> GetCustomers()
        {
            if (string.IsNullOrEmpty(_customers))
                return new List<CustomerDto>();
            _customerList = JsonConvert.DeserializeObject<List<CustomerDto>>(_customers);
            return _customerList;
        }
        public short? GetLastId()
        {
            if (string.IsNullOrEmpty(_customers))
                return null;
            short? customerId = JsonConvert.DeserializeObject<List<CustomerDto>>(_customers).OrderByDescending(x => x.Id).FirstOrDefault()?.Id;
            return customerId;
        }
    }
}
