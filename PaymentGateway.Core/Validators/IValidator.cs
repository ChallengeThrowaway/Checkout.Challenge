using System.Collections.Generic;


namespace PaymentGateway.Core.Validators
{
    public interface IValidator<T>
    {
        List<string> Validate(T objectToValidate);
    }
}
