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

            public const string Logout = $"{Base}/logout";

            public const string Me = $"{Base}/me";

            public const string RequestPasswordReset = $"{Base}/request-password-reset";

            public const string ResetPassword = $"{Base}/reset-password";

            public const string ConfirmEmail = $"{Base}/confirm-email";

            public const string ResendEmailCode = $"{Base}/resend-email-code";



        }

        public static class Accounts
        {

            public const string Base = $"{apiBase}/accounts";
            public const string GetAll = $"{Base}";
            public const string Activate = $"{Base}/{{id:guid}}/activate";
            public const string DeActivate = $"{Base}/{{id:guid}}/deactivate";
        }

        public static class Dashboards
        {
            public const string Base = $"{apiBase}/dashboards";
            public const string AccountStatistics = $"{Base}/account-statistics";

        }
    }
}
