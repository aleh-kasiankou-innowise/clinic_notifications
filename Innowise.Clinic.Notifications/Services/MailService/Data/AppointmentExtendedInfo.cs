namespace Innowise.Clinic.Notifications.Services.MailService.Data;

public class AppointmentExtendedInfo
{
    public AppointmentExtendedInfo(string patientName, string doctorName, string serviceName)
    {
        PatientName = patientName;
        DoctorName = doctorName;
        ServiceName = serviceName;
    }

    public string PatientName { get; }
    public string DoctorName { get; }
    public string ServiceName { get; }
}