namespace RW_ASPNET_EBANXTEST.Errors
{
    public class InvalidAccountException : Exception
    {
        public string account_id { get; }

        public InvalidAccountException(string _account_id)
        {
            account_id = _account_id;
        }
    }
}
