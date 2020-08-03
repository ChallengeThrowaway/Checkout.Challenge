
namespace PaymentGateway.Service.Extensions
{
    public static class StringExtensions
    {
        public static string FormatMaskedCardDetails(this string s)
        {
            return s.Replace(" ", "").Replace("-", "").Remove(4, 8).Insert(4, "********");
        }
    }
}
