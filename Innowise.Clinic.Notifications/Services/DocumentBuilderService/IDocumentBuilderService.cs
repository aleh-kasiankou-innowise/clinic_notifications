using Innowise.Clinic.Notifications.Services.MailService.Data;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.Services.DocumentBuilderService;

public interface IDocumentBuilderService
{
    string BuildBodyForEmailConfirmation(string emailConfirmationLink);
    string BuildBodyWithCredentials(EmployeeAccountGeneratedEvent userCredentials);
    string BuildBodyWithAppointmentReminder(AppointmentNotification appointmentRemindEventInfo,
        AppointmentExtendedInfo appointmentExtendedInfo);
    string BuildBodyWithAppointmentResult(AppointmentResultNotification appointmentResultChangeEventInfo,
        AppointmentExtendedInfo appointmentExtendedInfo);
}