namespace API
{
    #region using directives

    using System;

    #endregion using directives

    public sealed class GiftRoomApi
    {
        public const String Version = "v1.1";

        private static readonly ApiClient apiClient = new ApiClient();

        public static void Init(String endpoint, String appType)
        {
            apiClient.Init(endpoint, appType);
        }

        public static TService Create<TService>()
        {
            return apiClient.GetClient<TService>();
        }
    }
}