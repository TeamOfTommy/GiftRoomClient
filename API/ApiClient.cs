namespace API
{
    #region using directives

    using System;
    using System.Security.Permissions;

    #endregion using directives

    internal class ApiClient
    {
        private String appType;
        private String endpoint;

        internal void Init(String endpoint, String appType)
        {
            this.endpoint = endpoint;
            this.appType = appType;
        }

        internal TInterface GetClient<TInterface>()
        {
            var client = new ApiClient();
            return client.GetClient<TInterface>(this.endpoint, this.appType);
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        private TInterface GetClient<TInterface>(String endpoint, String appType)
        {
            var remoteProxy = new ApiProxy<TInterface>(endpoint, appType);
            return (TInterface)remoteProxy.GetTransparentProxy();
        }
    }
}