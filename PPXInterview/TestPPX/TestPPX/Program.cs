using PPX_Pos;
using System;
using System.Configuration;


namespace TestPPX
{
    /// <summary>
    /// Wrapper that adds country to welcome message
    /// </summary>
    public class CountryPOS : IPOS
    {
        private readonly IPOS _originalPos;
        private readonly string _country;

        public CountryPOS(IPOS originalPos, string country)
        {
            _originalPos = originalPos;
            _country = country;
        }

        public string DisplayWelcomeScreen()
        {
            string originalMessage = _originalPos.DisplayWelcomeScreen();
            return string.IsNullOrEmpty(_country)
                ? originalMessage
                : $"{originalMessage} {_country} customer";
        }

        public Guid CreateCustomerOrder() => _originalPos.CreateCustomerOrder();
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Read country from config
            string country = ConfigurationManager.AppSettings["CustomerCountry"] ?? "";

            IPOS pos = string.IsNullOrEmpty(country)
                ? new PassportX_POS()
                : new CountryPOS(new PassportX_POS(), country);

            POS_Process.Load(pos);

        }
    }


}
