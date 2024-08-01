using RW_ASPNET_EBANXTEST.Models;

namespace RW_ASPNET_EBANXTEST.Services.Results
{
    public class WithdrawResult : IMonetaryResult
    {
        public Account origin { get; }

        public WithdrawResult(Account origin)
        {
            this.origin = origin;
        }

    }
}
