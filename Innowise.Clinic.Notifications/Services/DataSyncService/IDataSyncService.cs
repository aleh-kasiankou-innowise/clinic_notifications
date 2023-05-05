using Innowise.Clinic.Notifications.Services.MailService.Data;

namespace Innowise.Clinic.Notifications.Services.DataSyncService;

public interface IDataService
{
    Task<AppointmentExtendedInfo> GetAppointmentExtendedInfoAsync(Guid doctorId, Guid patientId,
        Guid serviceId);

    Task<string?> TryGetValueFromCache(string key, string? newValue = null);
}