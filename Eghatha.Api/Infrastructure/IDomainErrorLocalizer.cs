using Eghatha.Domain.Teams;
using ErrorOr;
using Microsoft.Extensions.Localization;

namespace Eghatha.Api.Infrastructure
{
    public interface IDomainErrorLocalizer
    {
        string Localize(Error error);
    }

    public class DomainErrorLocalizer : IDomainErrorLocalizer
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly string _domainAssemblyName;

        public DomainErrorLocalizer(IStringLocalizerFactory factory)
        {
            _factory = factory;
            _domainAssemblyName = typeof(TeamErrors).Assembly.GetName().Name!;
        }

        public string Localize(Error error)
        {
            Console.WriteLine($"{_domainAssemblyName}");
            Console.WriteLine($"{error.Code}");
            Console.WriteLine($"{error.Code.Split('.')[0]}");


            
            var resourceClassName = error.Code.Split('.')[0];
          
           


            var localizer = _factory.Create(resourceClassName, _domainAssemblyName);
            var result = localizer[error.Code];

            return result.ResourceNotFound ? error.Description : result.Value;
        }
    }
}
