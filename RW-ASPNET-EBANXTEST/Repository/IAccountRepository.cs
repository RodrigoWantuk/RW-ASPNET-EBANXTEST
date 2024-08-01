using RW_ASPNET_EBANXTEST.Models;

namespace RW_ASPNET_EBANXTEST.Repository
{
    // Repository interface for accounts.
    // The idea behind creating an interface for handling model's repositories is
    // to provide an easy approach to migrate from different kinds of repos if needed,
    // such as Cached, SQL, Cloud, e.t.c.
    //
    // Rodrigo Wantuk, Aug-1st-2024
    public interface IAccountRepository
    {
        Account GetAccount(string account_id);
        Account CreateAccount(string account_id);
        void Reset();
    }
}
