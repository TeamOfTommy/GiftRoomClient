
namespace API
{
    #region using directives

    using System;

    #endregion using directives

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ApiAttribute : Attribute
    {
        public String Url { get; set; }

        public String HttpMethod { get; set; }

        public String ContentType { get; set; }
    }
}
