using FluentValidation;
using RestAPIAlgorithm.Model;

namespace RestAPIAlgorithm.Helper.Validation
{
    public class CustomerValidation:AbstractValidator<List<CustomerDto>>
    {
        public CustomerValidation()
        {
            RuleForEach(x => x)
                .Must((model) => !string.IsNullOrEmpty(model.FirstName)).WithMessage("FirstName can not be null");
            RuleForEach(x => x)
               .Must((model) => !string.IsNullOrEmpty(model.LastName)).WithMessage("LastName can not be null");
            RuleForEach(x => x)
            .Must((model) => model.Age >= 18).WithMessage("Age value cannot be less than 18");
        }
    }
}
