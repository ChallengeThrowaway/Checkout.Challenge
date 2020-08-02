using System.Collections.Generic;

namespace PaymentGateway.Service.Validators
{
    public interface IValidator<T>
    {
        List<string> Validate(T objectToValidate);
    }
}
