namespace Funda.Exceptions
{
    public class RetryLimitExceededException : Exception
    {
        public RetryLimitExceededException() { }

        public RetryLimitExceededException(string message)
            : base(message) { }
    }
}
