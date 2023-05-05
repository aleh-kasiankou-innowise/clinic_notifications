using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.Services.SchedulingHelperService;

public interface ISchedulingHelperService
{
    bool IsAppointmentReminderDueForScheduling(AppointmentNotification appointmentRemindEventInfo, out TimeSpan sendDelay);
}