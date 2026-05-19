namespace Eghatha.Contract.Volunteers.Responses
{
    public record volunteerRankingResponse(Guid VolunteerId,
   string FullName,
   string Speciality,
   string Province,
   string City,
   int TotalMissions,
   int TotalScore,
   double AverageScore,
   int Rank);
    
    
}
