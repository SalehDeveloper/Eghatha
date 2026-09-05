using ErrorOr;

namespace Eghatha.Application.Common.Errors
{
    public static class ApplicationErrors
    {
        public static readonly Error InvalidRefreshToken = Error.Unauthorized(
           code: "Auth.InvalidRefreshToken",
           description: Resources.ApplicationErrors.Auth_InvalidRefreshToken);

        public static readonly Error InvalidOtp = Error.Conflict(
           code: "Auth.InvalidOtp",
           description: Resources.ApplicationErrors.Auth_InvalidOtp);


        public static readonly Error EmailAlreadyConfirmed = Error.Conflict(
           code: "Auth.EmailAlreadyConfirmed",
           description: Resources.ApplicationErrors.Auth_EmailAlreadyConfirmed);

        public static readonly Error TeamNotFound = Error.NotFound(
            code: "Team.NotFound",
            description: Resources.ApplicationErrors.Team_NotFound);

        public static readonly Error TeamNotAvailable = Error.Conflict(
           code: "Team.NotAvailable",
           description: Resources.ApplicationErrors.Team_NotAvailable);



        public static readonly Error DisasterNotFound = Error.NotFound(
         code: "Disaster.NotFound",
         description: Resources.ApplicationErrors.Disaster_NotFound);


        public static readonly Error VolunteerNotFound = Error.NotFound(
            code: "Volunteer.NotFound",
            description: Resources.ApplicationErrors.Volunteer_NotFound);

        public static readonly Error DisasterResourceNotFound = Error.NotFound(
         code: "DisasterResourceNotFound.NotFound",
         description: Resources.ApplicationErrors.DisasterResourceNotFound_NotFound);

        public static readonly Error RegisterationNotFound = Error.NotFound(
           code: "RegisterationNotFound.NotFound",
           description: Resources.ApplicationErrors.RegisterationNotFound_NotFound);

        public static readonly Error UserWithEmailAlreadyExitst = Error.Conflict(
            code: "User.UserWithEmailAlreadyExitst",
            description: Resources.ApplicationErrors.User_UserWithEmailAlreadyExitst);


        public static readonly Error NotificationNotFound = Error.NotFound(
            code: "Notification.NotFound",
            description: Resources.ApplicationErrors.Notification_NotFound);

        public static readonly Error RefreshTokenExpired = Error.Conflict(
      code: "Auth.RefreshToken.Expired",
      description: Resources.ApplicationErrors.Auth_RefreshToken_Expired);

        public static readonly Error UserIdClaimInvalid = Error.Conflict(
    code: "Auth.UserIdClaim.Invalid",
    description: Resources.ApplicationErrors.Auth_UserIdClaim_Invalid);


        public static readonly Error ExpiredAccessTokenInvalid = Error.Conflict(
        code: "Auth.ExpiredAccessToken.Invalid",
        description: Resources.ApplicationErrors.Auth_ExpiredAccessToken_Invalid);

        public static readonly Error NoTeamCurrentDisaster = Error.NotFound(
            code: "Team.NoCurrentDisaster",
            description: Resources.ApplicationErrors.Team_NoCurrentDisaster);

        public static readonly Error NoVolunteerCurrentDisaster = Error.NotFound(
           code: "Volunteer.NoCurrentDisaster",
           description: Resources.ApplicationErrors.Volunteer_NoCurrentDisaster);

    }
}