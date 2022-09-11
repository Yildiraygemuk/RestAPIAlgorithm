using Newtonsoft.Json;
using Quartz;
using RestAPIAlgorithm.Business.Abstract;
using RestAPIAlgorithm.Model;
using RestSharp;

namespace RestAPIAlgorithm.Job
{
    public class RequestJob : IJob
    {
        private readonly ILogger<RequestJob> _logger;
        private readonly RestClient _client;
        private readonly ICustomerService _customerService;
        private static IConfiguration _configuration;
        private readonly List<string> _firstNames;
        private readonly List<string> _lastNames;
        public RequestJob(ILogger<RequestJob> logger, ICustomerService customerService, IConfiguration configuration)
        {
            _logger = logger;
            _client = new RestClient("https://localhost:5000/");
            _customerService = customerService;
            _configuration = configuration;
            _firstNames = _configuration.GetSection("FirstNames").Get<List<string>>();
            _lastNames = _configuration.GetSection("LastNames").Get<List<string>>();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Job worked...");
            //PostCustomers();

            //GetCustomers();
            _logger.LogInformation("Job stopped...");

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        public void GetCustomers()
        {
            try
            {
                var response = _client.Get(new RestRequest("/api/customer"));
                _logger.LogInformation("GET REQUEST => " + response.Content);
            }
            catch (Exception error)
            {
                _logger.LogInformation(error.Message);
            }
        }
        public void PostCustomers()
        {
            var entities = CreateEntities();
            var request = new RestRequest("/api/customer", Method.Post);
            request.AddParameter("application/json", entities, ParameterType.RequestBody);
            try
            {
                var response = _client.Execute(request);
                _logger.LogInformation("POST REQUEST => " + response.Content);
            }
            catch (Exception error)
            {
                _logger.LogInformation(error.Message);
            }
        }
        //A method that generates data randomly in the age range of 10-90 by taking the name and surname from appsettings and assigning them randomly and increasing the Id sequentially.
        public List<CustomerDto> CreateEntities()
        {
            short? lastId = _customerService.GetLastId() == null ? (short)1 : (short)(_customerService.GetLastId() + 1);
            Random rnd = new Random();
            List<CustomerDto> entities = new List<CustomerDto>();
            for (int i = 0; i < rnd.Next(2, 6); i++)
            {
                CustomerDto customer = new CustomerDto()
                {
                    FirstName = _firstNames[rnd.Next(_firstNames.Count)],
                    LastName = _lastNames[rnd.Next(_lastNames.Count)],
                    Age = (short)rnd.Next(10, 90),
                    Id = lastId.Value
                };
                lastId++;
                entities.Add(customer);
            }
            return entities;
        }
    }
}
