using RW_ASPNET_EBANXTEST.Models;

namespace RW_ASPNET_EBANXTEST.Services.Results
{
    public class DepositResult : IMonetaryResult
    {
        public Account destination { get; }

        public DepositResult(Account destination)
        {
            this.destination = destination;
        }

    }
}
