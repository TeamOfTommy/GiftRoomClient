namespace API
{
    #region using directives

    using System;

    #endregion using directives

    public class ApiException : Exception
    {
        public Int32 ErrorCode { get; set; }

        public ApiException()
        {
        }

        public ApiException(String message)
            : base(message)
        {
        }

        public ApiException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}