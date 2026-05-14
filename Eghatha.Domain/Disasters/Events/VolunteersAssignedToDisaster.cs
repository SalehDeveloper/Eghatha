using Eghatha.Domain.Abstractions;
using Eghatha.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Domain.Disasters.Events
{
    public sealed class VolunteersAssignedToDisaster:DomainEvent
    {
        public VolunteersAssignedToDisaster(Guid disasterId, List<Guid> volunteerIds, GeoLocation location, string city, string province, string disasterType, DateTimeOffset startTime, string title, string description)
        {
            DisasterId = disasterId;
            VolunteerIds = volunteerIds;
            Location = location;
            City = city;
            Province = province;
            DisasterType = disasterType;
            StartTime = startTime;
            Title = title;
            Description = description;
        }

        public Guid DisasterId { get; set; } 
        
        public List<Guid> VolunteerIds { get; set;}
        
        public GeoLocation Location { get; set; }

        public string City { get; set; }

        public string Province {  get; set; }

        public string DisasterType { get; set; }

        public DateTimeOffset StartTime {  get; set; }

        public string Title { get; set; }

        public string Description { get; set; }




    
    }
}
