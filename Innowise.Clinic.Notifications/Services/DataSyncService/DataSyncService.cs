using Innowise.Clinic.Notifications.Constants;
using Innowise.Clinic.Notifications.Services.MailService.Data;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace Innowise.Clinic.Notifications.Services.DataSyncService;

public class DataService : IDataService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IRequestClient<DoctorNameRequest> _doctorNameRequestClient;
    private readonly IRequestClient<PatientNameRequest> _patientNameRequestClient;
    private readonly IRequestClient<ServiceNameRequest> _serviceNameRequestClient;

    private readonly int _cacheDurationInHours =
        int.Parse(Environment.GetEnvironmentVariable("RedisOptions__NotificationsNameCacheDurationHours") ?? "1");

    public DataService(IDistributedCache distributedCache,
        IRequestClient<PatientNameRequest> patientNameRequestClient,
        IRequestClient<DoctorNameRequest> doctorNameRequestClient,
        IRequestClient<ServiceNameRequest> serviceNameRequestClient)
    {
        _distributedCache = distributedCache;
        _doctorNameRequestClient = doctorNameRequestClient;
        _patientNameRequestClient = patientNameRequestClient;
        _serviceNameRequestClient = serviceNameRequestClient;
    }

    private string BuildRedisKey(RedisKeys entityKey, Guid id) => $"{entityKey}-{id}";

    private async Task<string> GetDataByKeyAsync(RedisKeys entityKey, Guid id)
    {
        var key = BuildRedisKey(entityKey, id);
        return await _distributedCache.GetStringAsync(key) ?? await SyncEntityNameAsync(entityKey, id);
    }

    public async Task<AppointmentExtendedInfo> GetAppointmentExtendedInfoAsync(Guid doctorId, Guid patientId,
        Guid serviceId)
    {
        var doctorNameCacheCheckTask =
            GetDataByKeyAsync(RedisKeys.Doctor, doctorId);
        var patientNameCacheCheckTask =
            GetDataByKeyAsync(RedisKeys.Patient, patientId);
        var serviceNameCacheCheckTask =
            GetDataByKeyAsync(RedisKeys.Service, serviceId);

        await Task.WhenAll(doctorNameCacheCheckTask, patientNameCacheCheckTask, serviceNameCacheCheckTask);

        var doctorName = await doctorNameCacheCheckTask;
        var patientName = await patientNameCacheCheckTask;
        var serviceName = await serviceNameCacheCheckTask;

        return new AppointmentExtendedInfo(patientName, doctorName, serviceName);
    }

    public async Task<string?> TryGetValueFromCache(string key, string? newValue = null)
    {
        var currentValue = _distributedCache.GetStringAsync(key);
        if (newValue is not null)
        {
            await _distributedCache.SetStringAsync(key, newValue);
        }
        
        return await currentValue;
    }

    private async Task<string> SyncEntityNameAsync(RedisKeys entityKey, Guid id)
    {
        string name;

        switch (entityKey)
        {
            case RedisKeys.Doctor:
                name = (await _doctorNameRequestClient.GetResponse<DoctorNameResponse>(new(id)))
                    .Message
                    .DoctorFullName;
                break;
            case RedisKeys.Patient:
                name = (await _patientNameRequestClient.GetResponse<PatientNameResponse>(new(id)))
                    .Message
                    .PatientFullName;
                break;
            case RedisKeys.Service:
                name = (await _serviceNameRequestClient.GetResponse<ServiceNameResponse>(new(id)))
                    .Message
                    .ServiceName;
                break;
            default:
                throw new NotSupportedException(
                    $"There is no support for entity type: {nameof(RedisKeys)} - {entityKey}");
        }

        var key = BuildRedisKey(entityKey, id);
        await _distributedCache.SetStringAsync(key, name, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_cacheDurationInHours),
        });
        return name;
    }
}