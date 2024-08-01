using RW_ASPNET_EBANXTEST.Models;
using RW_ASPNET_EBANXTEST.Errors;

namespace RW_ASPNET_EBANXTEST.Repository
{
    public class CachedAccountRepository : IAccountRepository
    {
        // Create the repository as a singleton, to work as a memory cache, a memory repos.
        private static CachedAccountRepository s_instance;
        public static CachedAccountRepository GetInstance()
        {
            if (s_instance == null)
                s_instance = new CachedAccountRepository();

            return s_instance;
        }

        private List<Account> _accounts;

        private CachedAccountRepository()
        {
            _accounts = new List<Account>();
            Reset();
        }

        public Account GetAccount(string account_id)
        {
            try
            {
                return _accounts.First(c => c.id.Equals(account_id));
            }
            catch (InvalidOperationException)
            {
                throw new InvalidAccountException(account_id);
            }
        }

        public Account CreateAccount(string account_id)
        {
            Account _account;

            try
            {
                _account = GetAccount(account_id);
            }
            catch(InvalidAccountException)
            {
                _account = new Account();
                _account.id = account_id;
                _account.balance = 0;

                _accounts.Add(_account);
            }

            return _account;
        }

        public void Reset()
        {
            if (_accounts != null)
                _accounts.Clear();

            _accounts = new List<Account>();
        }
    }
}
