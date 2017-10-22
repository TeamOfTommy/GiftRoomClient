namespace API
{
    #region using directives

    using System;

    #endregion using directives

    public abstract class ApiModelBase
    {
        public string code { get; set; }

        public string message { get; set; }

        public string ts { get; set; }
    }
}