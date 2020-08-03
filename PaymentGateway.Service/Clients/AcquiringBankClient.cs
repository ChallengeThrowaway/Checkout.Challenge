using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGateway.Core.Configuration;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;

namespace PaymentGateway.Service.Clients
{
    public class AcquiringBankClient : IAcquiringBankClient
    {
        private readonly AcquiringBankSettings _acquiringBankSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<IAcquiringBankClient> _logger;

        public AcquiringBankClient(
            IOptions<AcquiringBankSettings> acquiringBankSettings,
            HttpClient httpClient,
            ILogger<AcquiringBankClient> logger)
        {
            _acquiringBankSettings = acquiringBankSettings.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AcquiringBankPaymentDetails> SubmitPaymentToBank(PaymentRequest paymentRequest)
        {
            HttpResponseMessage response;

            try
            {
                var data = JsonSerializer.Serialize(paymentRequest);
                var content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync($"{_acquiringBankSettings.BaseUrl}/api/v1/payments", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting payment request to acquiring bank");
                return GenerateErrorDetails();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            AcquiringBankResponse responseObject;

            try
            {
                responseObject = JsonSerializer.Deserialize<AcquiringBankResponse>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing response");
                return GenerateErrorDetails();

            }

            return GenerateDetails(responseObject);
        }

        private AcquiringBankPaymentDetails GenerateDetails(AcquiringBankResponse response)
        {
            return new AcquiringBankPaymentDetails
            {
                BankId = response.BankId,
                PaymentStatus = MapStatusFromString(response),
                StatusDateTime = response.StatusDateTime
            };
        }

        private AcquiringBankPaymentDetails GenerateErrorDetails()
        {
            return new AcquiringBankPaymentDetails
            {
                PaymentStatus = PaymentStatuses.SubmissionError,
                StatusDateTime = DateTime.UtcNow
            };
        }

        //TODO: Probably a better way to do this, should investigate a different approach. This would not be pleasant to update when new statuses are added, and doesn't really belong in this class.
        private PaymentStatuses MapStatusFromString(AcquiringBankResponse response)
        {
            if (response.PaymentStatus == null)
            {
                _logger.LogWarning($"Null status response from aqcuiring bank");

                return PaymentStatuses.SubmissionError;
            }

            if (response.PaymentStatus.Equals("ValidationError", StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentStatuses.BankValidationError;
            }

            if (response.PaymentStatus.Equals("SubmissionError", StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentStatuses.SubmissionError;
            }

            if (response.PaymentStatus.Equals("Submitted", StringComparison.InvariantCultureIgnoreCase))
            {
                return PaymentStatuses.Submitted;
            }

            _logger.LogWarning($"Unmapped status : {response.PaymentStatus} from acquiring bank");

            return PaymentStatuses.SubmissionError;
        }
    }
}
