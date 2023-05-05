using System.Text;
using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.MailService.Data;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.Services.DocumentBuilderService;

public class DocumentBuilderService : IDocumentBuilderService
{
    public string BuildBodyForEmailConfirmation(string emailConfirmationLink)
    {
        var emailConfirmationMessage = new StringBuilder(EmailTemplates.EmailConfirmation)
            .Replace(EmailVariables.EmailConfirmationLink, emailConfirmationLink);
        return emailConfirmationMessage.ToString();
    }

    public string BuildBodyWithCredentials(EmployeeAccountGeneratedEvent userCredentials)
    {
        var employeeAccountRegistrationMessage = new StringBuilder(EmailTemplates.EmailWithCredentials)
        .Replace(EmailVariables.EmailAddress, userCredentials.Email)
        .Replace(EmailVariables.Password, userCredentials.Password)
        .Replace(EmailVariables.Role, userCredentials.Role);
        return employeeAccountRegistrationMessage.ToString();
    }

    public string BuildBodyWithAppointmentReminder(AppointmentNotification appointmentRemindEventInfo,
        AppointmentExtendedInfo appointmentExtendedInfo)
    {
        var appointmentReminderMessage = new StringBuilder(EmailTemplates.EmailWithAppointmentReminder)
            .Replace(EmailVariables.AppointmentDate,
                appointmentRemindEventInfo.AppointmentDateTime.Date.ToShortDateString())
            .Replace(EmailVariables.AppointmentTime,
                appointmentRemindEventInfo.AppointmentDateTime.TimeOfDay.ToString())
            .Replace(EmailVariables.ServiceName,
                appointmentExtendedInfo.ServiceName)
            .Replace(EmailVariables.PatientFullName,
                appointmentExtendedInfo.PatientName)
            .Replace(EmailVariables.DoctorFullName,
                appointmentExtendedInfo.DoctorName);
        return appointmentReminderMessage.ToString();
    }

    public string BuildBodyWithAppointmentResult(AppointmentResultNotification appointmentResultChangeEventInfo,
        AppointmentExtendedInfo appointmentExtendedInfo)
    {
        var appointmentResultMessage = new StringBuilder(EmailTemplates.EmailWithAppointmentResulInfo)
            .Replace(EmailVariables.AppointmentDate,
                appointmentResultChangeEventInfo.AppointmentDateTime.Date.ToShortDateString())
            .Replace(EmailVariables.AppointmentTime,
                appointmentResultChangeEventInfo.AppointmentDateTime.TimeOfDay.ToString())
            .Replace(EmailVariables.Complaints,
                appointmentResultChangeEventInfo.Complaints)
            .Replace(EmailVariables.Conclusion,
                appointmentResultChangeEventInfo.Conclusion)
            .Replace(EmailVariables.Recommendations,
                appointmentResultChangeEventInfo.Recommendations)
            .Replace(EmailVariables.ServiceName,
                appointmentExtendedInfo.ServiceName)
            .Replace(EmailVariables.PatientFullName,
                appointmentExtendedInfo.PatientName)
            .Replace(EmailVariables.DoctorFullName,
                appointmentExtendedInfo.DoctorName);
        return appointmentResultMessage.ToString();
    }
}