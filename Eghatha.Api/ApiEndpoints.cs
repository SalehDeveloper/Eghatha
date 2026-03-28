using Asp.Versioning.Conventions;

namespace Eghatha.Api
{
    public static class ApiEndpoints
    {
        private const string apiBase = "api/v{version:apiVersion}";

        public static class Identity
        {
            public const string Base = $"{apiBase}/identity";

            public const string Login = $"{Base}/login";
            public const string RefreshToken = $"{Base}/refresh-token";
            public const string Me = $"{Base}/me";




        }
    }
}
