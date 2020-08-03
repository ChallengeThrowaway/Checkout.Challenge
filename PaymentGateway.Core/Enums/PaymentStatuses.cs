
namespace PaymentGateway.Core.Enums
{
    public enum PaymentStatuses
    {
        PendingSubmission = 0,
        Submitted = 1,
        SubmissionError = 2,
        InternalValidationError = 3,
        BankValidationError = 4
    }
}
