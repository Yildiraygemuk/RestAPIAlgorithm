using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPIAlgorithm.Business.Abstract;
using RestAPIAlgorithm.Model;

namespace RestAPIAlgorithm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<CustomerDto> customerDto)
        {
            var customer = await _customerService.AddCustomers(customerDto);
            return Ok(customer);
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<CustomerDto> customerDto = _customerService.GetCustomers();
            return Ok(customerDto);
        }
    }
}
