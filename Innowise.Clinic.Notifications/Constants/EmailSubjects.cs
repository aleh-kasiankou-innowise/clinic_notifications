namespace Innowise.Clinic.Notifications.Constants;

public static class EmailSubjects
{
    private const string SubjectTail = "| Your Clinic";
    public const string EmailConfirmation = $"Email Confirmation {SubjectTail}";
    public const string AdminProfileRegistration = $"Account Registration {SubjectTail}";
    public const string AppointmentReminder = $"Appointment Reminder {SubjectTail}";
    public const string AppointmentResultReady = $"Appointment Result Ready {SubjectTail}";
    public const string AppointmentResultUpdated = $"Appointment Result Updated {SubjectTail}";
}