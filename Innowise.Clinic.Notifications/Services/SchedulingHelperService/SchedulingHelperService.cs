using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Notifications.Services.SchedulingHelperService;

public class SchedulingHelperService : ISchedulingHelperService
{
    private readonly int _hoursBeforeAppointmentToSendReminder = int.Parse(Environment.GetEnvironmentVariable(
        "AppointmentNotifications__SyncImmediatelyIfLessHoursBeforeAppointment") ?? "24");

    public bool IsAppointmentReminderDueForScheduling(AppointmentNotification appointmentRemindEventInfo,
        out TimeSpan sendDelay)
    {
        var actualHoursBeforeAppointment = (appointmentRemindEventInfo.AppointmentDateTime - DateTime.Now).TotalHours;
        sendDelay = TimeSpan.FromHours(actualHoursBeforeAppointment - _hoursBeforeAppointmentToSendReminder);
        return actualHoursBeforeAppointment < _hoursBeforeAppointmentToSendReminder;
    }
}