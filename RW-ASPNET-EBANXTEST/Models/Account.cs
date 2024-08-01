namespace RW_ASPNET_EBANXTEST.Models
{
    // Model representing an account.
    // Simply using and unsigned int for account_id
    // and a decimal type for balance, since decimal is the best option for monetary ops in .NET.
    //
    // Rodrigo Wantuk, Aug-1st-2024
    public class Account
    {
        public string id { get; set; }
        public decimal balance { get; set; }
    }
}
