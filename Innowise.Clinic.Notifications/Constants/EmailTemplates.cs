namespace Innowise.Clinic.Notifications.Constants;

public static class EmailTemplates
{
    public const string EmailFooter = "<p>Your Clinic - To healthier future together!<p>";
    public const string SiteName = "clinic.com";

    public const string EmailConfirmation =
        "<p>Dear Customer, we've recieved your registration request." +
        " To confirm your email address and proceed with profile creation, please follow the link below:<br>" +
        $"<a href='{EmailVariables.EmailConfirmationLink}'>Confirm Email!</a></p>" +
        "<p>In case you haven't registered an account, please ignore this message.</p>" +
        EmailFooter;

    public const string EmailWithCredentials =
        $"<p>Dear Customer, the {EmailVariables.Role} account has been created for you at {SiteName}</p>" +
        $"<p>To log in, please use the following credentials:<br><em>Email: {EmailVariables.EmailAddress}</em>" +
        $"<br><em>Password: {EmailVariables.Password}</em></p>" +
        EmailFooter;

    public const string EmailWithAppointmentReminder =
        $"<p>Dear Customer, we kindly remind you about the upcoming appointment:</p>" +
        $"<p>Service: {EmailVariables.ServiceName}</p>" +
        $"<p>Patient: {EmailVariables.PatientFullName}</p>" +
        $"<p>Date: {EmailVariables.AppointmentDate}</p>" +
        $"<p>Time: {EmailVariables.AppointmentTime}</p>" +
        $"<p>Doctor: {EmailVariables.DoctorFullName}</p>" +
        $"<p>In case the information is incorrect, please contact our support team.</p>" +
        EmailFooter;


    public const string EmailWithAppointmentResulInfo =
        "<p>Dear customer, we'd like to inform you about new information available in your account. " +
        $"Your doctor has updated the results of your appoinment:</p>" +
        $"<p>Service: {EmailVariables.ServiceName}</p>" +
        $"<p>Patient: {EmailVariables.PatientFullName}</p>" +
        $"<p>Date: {EmailVariables.AppointmentDate}</p>" +
        $"<p>Time: {EmailVariables.AppointmentTime}</p>" +
        $"<p>Doctor: {EmailVariables.DoctorFullName}</p>" +
        "<p>Here are the up-to-date results of the checkup:</p>" +
        $"<p>Complaints: {EmailVariables.Complaints}</p>" +
        $"<p>Conclusion: {EmailVariables.Conclusion}</p>" +
        $"<p>Recommendations: {EmailVariables.Recommendations}</p>" + 
        EmailFooter;
}