namespace RapidPay.Services.Fees
{
    public class UniversalFeesExchange
    {
        private static readonly Lazy<UniversalFeesExchange> _instance = new Lazy<UniversalFeesExchange>(() => new UniversalFeesExchange());
        private decimal _currentFee = 1.0m;
        private readonly Random _random = new Random();

        private UniversalFeesExchange()
        {
            Task.Run(UpdateFee);
        }

        public static UniversalFeesExchange Instance => _instance.Value;

        public decimal GetCurrentFee() => _currentFee;

        private async Task UpdateFee()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromHours(1));
                _currentFee *= (decimal)(_random.NextDouble() * 2);
            }
        }
    }

}
