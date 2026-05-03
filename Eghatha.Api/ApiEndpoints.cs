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

        public static class Teams
        {
            public const string Base = $"{apiBase}/teams";

            public const string Create = $"{Base}";

            public const string Update = $"{Base}/{{teamid:guid}}";

            public const string GetAll = $"{Base}";

            public const string GetById = $"{Base}/{{teamid:guid}}";

            public const string GetTeamMembers = $"{Base}/{{teamid:guid}}/members";
            public const string GetTeamResources = $"{Base}/{{teamid:guid}}/resources";


            public const string UpdateLiveLocation = $"{Base}/{{teamid:guid}}/live-location";

            public const string AddMemeber =$"{Base}/{{teamid:guid}}/members";
            public const string ChangeLeader = $"{Base}/{{teamid:guid}}/leader/{{memberid:guid}}";

            public const string Activate = $"{Base}/{{teamid:guid}}/activate";
            public const string DeActivate = $"{Base}/{{teamid:guid}}/deactivate";
            

            public const string DeactivateMember = $"{Base}/{{teamid:guid}}/members/{{memberid:guid}}/deactivate";
            public const string ActivateMember = $"{Base}/{{teamid:guid}}/members/{{memberid:guid}}/activate";
            public const string OffDutyMemberStatus = $"{Base}/{{teamid:guid}}/members/{{memberid:guid}}/off-duty";
            public const string OnMissionMemberStatus = $"{Base}/{{teamid:guid}}/members/{{memberid:guid}}/on-mission";

            //Resources
            public const string AddResource = $"{Base}/{{teamid:guid}}/resources"; 
            public const string IncreaseResourceQuantity = $"{Base}/{{teamid:guid}}/resources/{{resourceid:guid}}/increase";
            public const string DecreaseResourceQuantity = $"{Base}/{{teamid:guid}}/resources/{{resourceid:guid}}/decrease";


        }

        public static class VolunteerRegisterations
        {
            public const string Base = $"{apiBase}/volunteer-registerations";

            public const string GetAll = $"{Base}";
            public const string GetById = $"{Base}/{{registerationid:guid}}";
            public const string Approve = $"{Base}/{{registerationid:guid}}/approve";
            public const string Reject = $"{Base}/{{registerationid:guid}}/reject";


        }

        public static class Volunteers
        {
            public const string Base = $"{apiBase}/volunteers";

            public const string Create = $"{Base}";

            public const string GetAll = $"{Base}";

            public const string GetById = $"{Base}/{{volunteerid:guid}}";
            public const string volunteerBusy = $"{Base}/{{volunteerid:guid}}/busy";
            public const string volunteerAvailable = $"{Base}/{{volunteerid:guid}}/available";
            public const string volunteerUnAvailable = $"{Base}/{{volunteerid:guid}}/unavailable";
            public const string UpdateLocation = $"{Base}/{{volunteerid:guid}}/location";
            public const string IncreaseEquipmentQuantity = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}/increase";
            public const string DecreaseEquipmentQuantity = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}/decrease";
            public const string UpdateEquipment = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}";
            public const string EquipmentValid = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}/valid";
            public const string EquipmentInValid = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}/invalid";
            public const string RemoveEquipment = $"{Base}/{{volunteerid:guid}}/equipments/{{equipmentid:guid}}";

            public const string AddEquipment = $"{Base}/{{volunteerid:guid}}/equipments";
            public const string GetVolunteerEquipments = $"{Base}/{{volunteerid:guid}}/equipments";


        }
    }
}
