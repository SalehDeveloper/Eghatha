using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Disasters;
using Eghatha.Domain.Shared.ValueObjects;

namespace Eghatha.Domain.Disaster
{
    public sealed class DisasterCreated : DomainEvent
    {
        public Guid Id { get; set;}

        public double Latitude { get; set;}

        public double Longitude { get; set;}


        public DisasterType Type { get; set;}

        public string? CustomeDescription { get; set;}  

        public DateTimeOffset OccuredAt { get; set;}    

          
          public DisasterCreated( Guid id , double latitude , double longitude , DisasterType type , string? customeDescription , DateTimeOffset occuredAt)
          {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
            Type = type;
            CustomeDescription = customeDescription;
            OccuredAt = occuredAt;  
          }
    }
}