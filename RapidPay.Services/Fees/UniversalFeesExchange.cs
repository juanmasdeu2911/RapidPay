using RapidPay.Services.Interfaces;

namespace RapidPay.Services.Fees
{
    public class UniversalFeesExchangeService : IUniversalFeesExchangeService
    {
        private decimal _currentFee = 1.0m;
        private readonly Random _random = new Random();
        private static readonly object _lock = new object();
        private readonly Func<Task> _delayFunction;

        public UniversalFeesExchangeService() : this(new Random(), () => Task.Delay(TimeSpan.FromHours(1)))
        {
        }

        public UniversalFeesExchangeService(Random random, Func<Task> delayFunction)
        {
            _random = random;
            _delayFunction = delayFunction;
            Task.Run(UpdateFee);
        }

        public UniversalFeesExchangeService(int seed, Func<Task> delayFunction) : this(new Random(seed), delayFunction)
        {
        }


        public decimal GetCurrentFee()
        {
            lock (_lock)
            {
                return _currentFee;
            }
        }

        private async Task UpdateFee()
        {
            while (true)
            {
                await _delayFunction();
                _currentFee *= (decimal)(_random.NextDouble() * 2);
            }
        }
    }
}
