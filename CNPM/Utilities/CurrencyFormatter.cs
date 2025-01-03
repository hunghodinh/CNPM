using System.Globalization;

namespace CNPM.Utilities
{
    public class CurrencyFormatter
    {
        public static string FormatVND(decimal? amount)
        {
            if (amount.HasValue)
            {
                return string.Format(new CultureInfo("vi-VN"), "{0:C0}", amount.Value);
            }
            return string.Empty;
        }
    }
}
