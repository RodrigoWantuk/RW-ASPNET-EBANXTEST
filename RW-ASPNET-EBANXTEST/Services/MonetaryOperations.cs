using RW_ASPNET_EBANXTEST.Repository;
using RW_ASPNET_EBANXTEST.Models;
using RW_ASPNET_EBANXTEST.Errors;
using RW_ASPNET_EBANXTEST.Services.Results;

namespace RW_ASPNET_EBANXTEST.Services
{
    public class MonetaryOperations
    {
        private IAccountRepository _repos;

        public MonetaryOperations(IAccountRepository repos)
        {
            _repos = repos;
        }

        public void Reset()
        {
            _repos.Reset();
        }

        public bool AccountExists(string account_id)
        {
            try
            {
                _repos.GetAccount(account_id);
                return true;
            }
            catch (InvalidAccountException)
            {
            }
            return false;
        }

        public decimal GetBalance(string account_id)
        {
            return _repos.GetAccount(account_id).balance;
        }

        public IMonetaryResult Deposit(string account_id, decimal d_value)
        {
            if (!AccountExists(account_id))
                _repos.CreateAccount(account_id); // Just for deposit and transfer operation, will create an account if it does not exists.

            Account destination = _repos.GetAccount(account_id);
            destination.balance += d_value;
            return new DepositResult(destination);
        }

        public IMonetaryResult Withdraw(string account_id, decimal w_value)
        {
            Account origin = _repos.GetAccount(account_id);
            origin.balance -= w_value;
            return new WithdrawResult(origin);
        }

        public IMonetaryResult Transfer(string origin_account_id, string destination_account_id, decimal t_value)
        {
            if (!AccountExists(destination_account_id))
                _repos.CreateAccount(destination_account_id); // Just for deposit and transfer operation, will create an account if it does not exists.

            Account origin = _repos.GetAccount(origin_account_id);
            Account destination = _repos.GetAccount(destination_account_id);
            origin.balance -= t_value;
            destination.balance += t_value;
            return new TransferResult(origin, destination);
        }

        public IMonetaryResult ProcessRESTEvent(PostEvent eventData)
        {
            // In case any account cannot be found, an InvalidAccountException will be trown up.
            IMonetaryResult result = null;
            switch (eventData.type.ToUpper())
            {
                case "DEPOSIT":
                    result = Deposit(eventData.destination, eventData.amount);
                    break;
                case "WITHDRAW":
                    result = Withdraw(eventData.origin, eventData.amount);
                    break;
                case "TRANSFER":
                    result = Transfer(eventData.origin, eventData.destination, eventData.amount);
                    break;
            }
            return result;
        }
    }
}
