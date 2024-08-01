using RW_ASPNET_EBANXTEST.Models;

namespace RW_ASPNET_EBANXTEST.Services.Results
{
    public class TransferResult : IMonetaryResult
    {
        public Account origin { get; }
        public Account destination { get; }

        public TransferResult(Account origin, Account destination)
        {
            this.origin = origin;
            this.destination = destination;
        }

    }
}
