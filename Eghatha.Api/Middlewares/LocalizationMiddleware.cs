using FluentValidation;
using System.Globalization;

namespace Eghatha.Api.Middlewares
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly string[] SupportedCultures = { "en", "ar" };
        private const string DefaultCulture = "en";

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var culture = context.Request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrEmpty(culture) || !SupportedCultures.Contains(culture))
            {
                culture = DefaultCulture;
            }

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(culture);

            await _next(context);
        }
    }
}
